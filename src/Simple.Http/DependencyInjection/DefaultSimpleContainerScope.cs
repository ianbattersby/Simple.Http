// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSimpleContainerScope.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the DefaultSimpleContainerScope type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.DependencyInjection
{
    using System;
    using System.Linq;

    using Simple.Http.Helpers;

    internal class DefaultSimpleContainerScope : ISimpleContainerScope
    {
        public T Get<T>()
        {
            if (typeof(T).IsInterface || typeof(T).IsAbstract)
            {
                T instance;

                if (TryCreateInstance(out instance))
                {
                    return instance;
                }

                throw new InvalidOperationException("No IoC Container found. Install a Simple.Http IoC container such as Simple.Http.Ninject.");
            }

            try
            {
                return Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {
                throw new InvalidOperationException("No IoC Container found. Install a Simple.Http IoC container such as Simple.Http.Ninject.");
            }
        }

        private static bool TryCreateInstance<T>(out T instance)
        {
            var implementations = ExportedTypeHelper.FromCurrentAppDomain(IsImplementationOf<T>).ToList();
            
            if (implementations.Count == 1)
            {
                if (implementations[0].GetConstructor(new Type[0]) != null)
                {
                    {
                        instance = (T)Activator.CreateInstance(implementations[0]);
                        return true;
                    }
                }
            }

            instance = default(T);
            
            return false;
        }

        private static bool IsImplementationOf<T>(Type type)
        {
            return (!(type.IsInterface || type.IsAbstract)) && typeof(T).IsAssignableFrom(type);
        }

        public void Dispose()
        {
        }
    }
}