// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BehaviorInfo.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the BehaviorInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Behaviors;
    using Helpers;
    using Protocol;

    internal abstract class BehaviorInfo
    {
        private readonly Type behaviorType;
        private readonly Type implementingType;
        private readonly Priority priority;

        protected BehaviorInfo(Type behaviorType, Type implementingType, Priority priority)
        {
            this.behaviorType = behaviorType;
            this.implementingType = implementingType;
            this.priority = priority;
        }

        public Priority Priority
        {
            get { return this.priority; }
        }

        public Type ImplementingType
        {
            get { return this.implementingType; }
        }

        public Type BehaviorType
        {
            get { return this.behaviorType; }
        }

        public bool Universal { get; set; }

        public MethodInfo GetMethod()
        {
            foreach (var methodInfo in this.ImplementingType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var parameters = methodInfo.GetParameters();

                if (parameters.Length != 2 || parameters[1].ParameterType != typeof(IContext))
                {
                    continue;
                }

                if (parameters[0].ParameterType == this.BehaviorType)
                {
                    return methodInfo;
                }

                if (!parameters[0].ParameterType.IsGenericType || !this.BehaviorType.IsGenericType)
                {
                    continue;
                }

                if (parameters[0].ParameterType.GetGenericTypeDefinition() == this.BehaviorType.GetGenericTypeDefinition())
                {
                    return methodInfo;
                }
            }

            throw new MissingMethodException(this.ImplementingType.Name, "Implementation");
        }

        public MethodInfo GetMethod(Type[] genericTypes)
        {
            foreach (var methodInfo in this.ImplementingType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var parameters = methodInfo.GetParameters();
                if (parameters.Length != 2)
                {
                    continue;
                }

                if (parameters[1].ParameterType != typeof(IContext))
                {
                    continue;
                }

                if (methodInfo.GetGenericArguments().Length != genericTypes.Length)
                {
                    continue;
                }

                return methodInfo.MakeGenericMethod(genericTypes);
            }

            throw new MissingMethodException(this.ImplementingType.Name, "Implementation");
        }

        protected static IEnumerable<TInfo> FindBehaviorTypes<TAttribute, TInfo>(Func<Type, Type, Priority, TInfo> construct)
            where TAttribute : BehaviorAttribute
        {
            // Method group not used to avoid issue with Mono
            foreach (var behaviorType in ExportedTypeHelper.FromCurrentAppDomain(type => IsBehaviorType<TAttribute>(type)))
            {
                var attribute = (TAttribute)Attribute.GetCustomAttribute(behaviorType, typeof(TAttribute));

                if (attribute != null)
                {
                    yield return construct(behaviorType, attribute.ImplementingType, attribute.Priority);
                }
            }
        }

        private static bool IsBehaviorType<T>(Type type)
            where T : BehaviorAttribute
        {
            return Attribute.GetCustomAttribute(type, typeof(T)) != null;
        }
    }
}