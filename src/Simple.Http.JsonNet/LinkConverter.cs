// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkConverter.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the LinkConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.JsonNet
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Simple.Http.Links;

    public static class LinkConverter
    {
        internal static readonly MethodInfo WritePropertyName = typeof(JsonWriter).GetMethod("WritePropertyName", new[] { typeof(string) });
        internal static readonly MethodInfo Serialize = typeof(JsonSerializer).GetMethod("Serialize", new[] { typeof(JsonWriter), typeof(object) });
        private static readonly ConcurrentDictionary<Type, JsonConverter> Converters = new ConcurrentDictionary<Type, JsonConverter>();
        private static readonly ConcurrentDictionary<Type, JsonConverter[]> ConverterSets = new ConcurrentDictionary<Type, JsonConverter[]>();

        public static JsonConverter Create(Type type, Func<object, IEnumerable<object>> linkEnumerator, IContractResolver contractResolver)
        {
            return Converters.GetOrAdd(type, t => Build(t, linkEnumerator, contractResolver));
        }

        public static JsonConverter[] CreateForGraph(Type type, HashSet<Type> knownTypes, Func<object, IEnumerable<object>> linkEnumerator, IContractResolver contractResolver)
        {
            return ConverterSets.GetOrAdd(type, t => CreateForGraphImpl(t, knownTypes, linkEnumerator, contractResolver));
        }

        private static JsonConverter[] CreateForGraphImpl(Type type, HashSet<Type> knownTypes, Func<object, IEnumerable<object>> linkEnumerator, IContractResolver contractResolver)
        {
            var list = new List<JsonConverter> { Create(type, linkEnumerator, contractResolver) };

            Add(type, list, knownTypes, linkEnumerator, new HashSet<Type>(), contractResolver);

            return list.ToArray();
        }

        private static void Add(Type type, IList<JsonConverter> converters, HashSet<Type> knownTypes, Func<object, IEnumerable<object>> linkEnumerator, HashSet<Type> done, IContractResolver contractResolver)
        {
            if (knownTypes.Contains(type))
            {
                converters.Add(Create(type, linkEnumerator, contractResolver));
            }

            done.Add(type);

            foreach (var property in type.GetProperties().Where(p => (!Ignore(p.PropertyType)) && (!done.Contains(p.PropertyType))))
            {
                Add(property.PropertyType, converters, knownTypes, linkEnumerator, done, contractResolver);
            }
        }

        internal static bool Ignore(Type type)
        {
            return type.IsPrimitive || type.IsArray || type == typeof(string) || type == typeof(Guid) || type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) || type.Name == "Nullable`1";
        }

        private static JsonConverter Build(Type type, Func<object, IEnumerable<object>> linkEnumerator, IContractResolver contractResolver)
        {
            var enumerable = type.GetInterface(typeof(IEnumerable<>).FullName);

            if (enumerable != null)
            {
                return Build(enumerable.GetGenericArguments().Single(), linkEnumerator, contractResolver);
            }

            var concreteType = typeof(LinkConverter<>).MakeGenericType(type);
            var newMethod = concreteType.GetMethod("New", BindingFlags.Static | BindingFlags.Public);

            return (JsonConverter)newMethod.Invoke(null, new object[] { linkEnumerator, contractResolver });
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    internal class LinkConverter<T> : JsonConverter
    {
        private readonly Action<JsonWriter, T, JsonSerializer> writeProperties;
        private readonly Func<object, IEnumerable<object>> linkEnumerator;

        private LinkConverter(Action<JsonWriter, T, JsonSerializer> writeProperties, Func<object, IEnumerable<object>> linkEnumerator)
        {
            this.writeProperties = writeProperties;
            this.linkEnumerator = linkEnumerator;
        }

        public static LinkConverter<T> New(Func<object, IEnumerable<object>> linkEnumerator, IContractResolver contractResolver)
        {
            var writer = Expression.Parameter(typeof(JsonWriter));
            var value = Expression.Parameter(typeof(T));
            var serializer = Expression.Parameter(typeof(JsonSerializer));

            var setters = CreateWriteValueExpressions(writer, value, serializer, contractResolver);

            var block = Expression.Block(setters);
            var lambda = Expression.Lambda<Action<JsonWriter, T, JsonSerializer>>(block, writer, value, serializer);

            return new LinkConverter<T>(lambda.Compile(), linkEnumerator);
        }

        private static IEnumerable<Expression> CreateWriteValueExpressions(ParameterExpression writer, ParameterExpression value, ParameterExpression serializer, IContractResolver contractResolver)
        {
            var contract = contractResolver.ResolveContract(typeof(T)) as JsonObjectContract;
            if (contract != null)
            {
                foreach (var property in contract.Properties)
                {
                    var getValueMethod = typeof(IValueProvider).GetMethod("GetValue");
                    var valueProvider = Expression.Constant(property.ValueProvider);

                    yield return
                        Expression.Call(
                            writer,
                            LinkConverter.WritePropertyName,
                            Expression.Constant(property.PropertyName));

                    var getValue = Expression.Call(valueProvider, getValueMethod, value);

                    yield return Expression.Call(serializer, LinkConverter.Serialize, writer, getValue);
                }
            }
            else
            {
                foreach (var info in typeof(T).GetProperties())
                {
                    yield return
                        Expression.Call(
                            writer,
                            LinkConverter.WritePropertyName,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                            Expression.Constant(ToCamelCase(info.Name)));

                    var getValue = Expression.Convert(Expression.Property(value, info), typeof(object));

                    yield return Expression.Call(serializer, LinkConverter.Serialize, writer, getValue);
                }
            }
        }

        private static string ToCamelCase(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLowerInvariant();
            }

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            this.writeProperties(writer, (T)value, serializer);

            writer.WritePropertyName("links");

            var links = this.linkEnumerator(value);

            foreach (var link in links.OfType<Link>())
            {
                if (string.IsNullOrWhiteSpace(link.Type))
                {
                    link.Type = "application/json";
                }
                else if (!link.Type.EndsWith("+json"))
                {
                    link.Type = link.Type + "+json";
                }
            }

            serializer.Serialize(writer, links);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }
    }
}