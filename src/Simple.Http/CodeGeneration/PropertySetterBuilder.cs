// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertySetterBuilder.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the PropertySetterBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal sealed class PropertySetterBuilder
    {
        private static readonly MethodInfo DictionaryContainsKeyMethod = typeof(IDictionary<string, string>).GetMethod("ContainsKey", new[] { typeof(string) });
        private static readonly PropertyInfo DictionaryIndexerProperty = typeof(IDictionary<string, string>).GetProperty("Item");

        private readonly ParameterExpression param;
        private readonly Expression obj;
        private readonly PropertyInfo property;
        private MemberExpression nameProperty;
        private Expression itemProperty;
        private MethodCallExpression containsKey;

        public PropertySetterBuilder(ParameterExpression param, Expression obj, PropertyInfo property)
        {
            this.param = param;
            this.obj = obj;
            this.property = property;
        }

        public ConditionalExpression CreatePropertySetter()
        {
            this.CreatePropertyExpressions();

            if (PropertyIsPrimitive())
            {
                return Expression.IfThen(this.containsKey, this.CreateTrySimpleAssign());
            }

            return null;
        }

        private bool PropertyIsPrimitive()
        {
            return PropertyIsPrimitive(this.property);
        }

        public static bool PropertyIsPrimitive(PropertyInfo property)
        {
            return TypeIsPrimitive(property.PropertyType);
        }

        private static bool TypeIsPrimitive(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(DateTimeOffset)
                   || type == typeof(Guid) || type == typeof(byte[]) || type.IsEnum
                   || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                   || IsPrimitiveEnumerable(type);
        }

        private static bool IsPrimitiveEnumerable(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) && TypeIsPrimitive(type.GetGenericArguments()[0]);
        }

        private void CreatePropertyExpressions()
        {
            var constName = Expression.Constant(this.property.Name, typeof(string));
            this.containsKey = Expression.Call(this.param, DictionaryContainsKeyMethod, constName);
            this.nameProperty = Expression.Property(this.obj, this.property);
            this.itemProperty = Expression.Property(this.param, DictionaryIndexerProperty, constName);
        }

        private CatchBlock CreateCatchBlock()
        {
            return Expression.Catch(
                typeof(Exception),
                Expression.Assign(this.nameProperty, Expression.Default(this.property.PropertyType)));
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed. Suppression is OK here.")]
        private TryExpression CreateTrySimpleAssign()
        {
            var changeTypeMethod = typeof(PropertySetterBuilder).GetMethod(
                "SafeConvert", BindingFlags.Static | BindingFlags.NonPublic);

            MethodCallExpression callConvert;
            if (this.property.PropertyType.IsEnum)
            {
                callConvert = Expression.Call(
                    changeTypeMethod, this.itemProperty, Expression.Constant(this.property.PropertyType.GetEnumUnderlyingType()));
            }
            else if (this.property.PropertyType.IsNullable())
            {
                callConvert = Expression.Call(
                    changeTypeMethod,
                    this.itemProperty,
                    Expression.Constant(this.property.PropertyType.GetGenericArguments().Single()));
            }
            else if (IsEnumerable(this.property.PropertyType))
            {
                changeTypeMethod = typeof(PropertySetterBuilder).GetMethod(
                    "SafeConvertEnumerable", BindingFlags.Static | BindingFlags.NonPublic);
                callConvert = Expression.Call(
                    changeTypeMethod, this.itemProperty, Expression.Constant(this.property.PropertyType));
            }
            else
            {
                callConvert = Expression.Call(
                    changeTypeMethod, this.itemProperty, Expression.Constant(this.property.PropertyType));
            }

            var assign = Expression.Assign(this.nameProperty, Expression.Convert(callConvert, this.property.PropertyType));
            if (this.property.PropertyType.IsEnum)
            {
                return Expression.TryCatch(
                    Expression.IfThenElse(
                        Expression.TypeIs(this.itemProperty, typeof(string)),
                        Expression.Assign(
                                        this.nameProperty, 
                                        Expression.Convert(
                                            Expression.Call(
                                                typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string), typeof(bool) }),
                                                Expression.Constant(this.property.PropertyType),
                                                Expression.Call(this.itemProperty, typeof(object).GetMethod("ToString")),
                                                Expression.Constant(true)),
                                                this.property.PropertyType)),
                            assign),
                        Expression.Catch(typeof(Exception), Expression.Empty()));
            }

            if (IsAGuid(this.property.PropertyType))
            {
                return Expression.TryCatch(
                    Expression.IfThenElse(
                        Expression.TypeIs(this.itemProperty, typeof(string)),
                        Expression.Assign(
                            this.nameProperty,
                            Expression.Convert(
                                Expression.Call(
                                    typeof(Guid).GetMethod("Parse", new[] { typeof(string) }),
                                    Expression.Call(this.itemProperty, typeof(object).GetMethod("ToString"))),
                                this.property.PropertyType)),
                        assign),
                    Expression.Catch(typeof(Exception), Expression.Empty()));
            }

            return Expression.TryCatch(
                assign, this.CreateCatchBlock());
        }

        private static object SafeConvert(object source, Type targetType)
        {
            return ReferenceEquals(source, null) ? null : Convert.ChangeType(source, targetType);
        }

        private static object SafeConvertEnumerable(object source, Type targetType)
        {
            if (source == null)
            {
                return null;
            }

            var tmpSource = (string)source;

            var destinationType = targetType.GetGenericArguments()[0];
            var collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(destinationType));
            if (!tmpSource.Contains("\t"))
            {
                // Single value IEnumerable element
                collection.Add(Cast(source, destinationType));
            }
            else
            {
                // Multi value IEnumerable element
                var parts = tmpSource.Split('\t');
                foreach (var part in parts)
                {
                    collection.Add(Cast(part, destinationType));
                }
            }

            return (IEnumerable)collection;
        }

        private static object Cast(object value, Type destinationType)
        {
            if (IsAGuid(destinationType))
            {
                var tmp = (string)value;
                return Guid.Parse(tmp);
            }

            if (destinationType.IsEnum)
            {
                var tmp = (string)value;
                return Enum.Parse(destinationType, tmp, true);
            }

            return Convert.ChangeType(value, destinationType);
        }

        private static bool IsAGuid(Type type)
        {
            return type == typeof(Guid) || type == typeof(Guid?);
        }

        private static bool IsEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type)
                   && (typeof(string) != type);
        }

        public static BlockExpression MakePropertySetterBlock(
            Type type,
            ParameterExpression variables,
            ParameterExpression instance,
            BinaryExpression construct)
        {
            var lines = new List<Expression> { construct };

            var setters = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Where(PropertyIsPrimitive)
                .Select(p => new PropertySetterBuilder(variables, instance, p))
                .Select(ps => ps.CreatePropertySetter());

            lines.AddRange(setters);
            lines.Add(instance);

            var block = Expression.Block(type, new[] { instance }, lines);
            return block;
        }

        public static BlockExpression MakePropertySetterBlock(
            Type type,
            MethodCallExpression getVariables,
            ParameterExpression instance,
            BinaryExpression construct)
        {
            var variables = Expression.Variable(typeof(IDictionary<string, string[]>), "variables");
            var lines = new List<Expression> { construct, Expression.Assign(variables, getVariables) };

            var setters = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Where(PropertyIsPrimitive)
                .Select(p => new PropertySetterBuilder(variables, instance, p))
                .Select(ps => ps.CreatePropertySetter());

            lines.AddRange(setters);
            lines.Add(instance);

            var block = Expression.Block(type, new[] { variables, instance }.AsEnumerable(), lines.AsEnumerable());
            return block;
        }
    }
}