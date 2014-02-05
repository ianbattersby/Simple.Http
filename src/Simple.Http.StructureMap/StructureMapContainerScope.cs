// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapContainerScope.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the StructureMapContainerScope type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.StructureMap
{
    using Simple.Http.DependencyInjection;

    using global::StructureMap;

    public class StructureMapContainerScope : ISimpleContainerScope
    {
        private readonly IContainer container;

        internal StructureMapContainerScope(IContainer container)
        {
            this.container = container;
        }

        public T Get<T>()
        {
            return IsConcrete<T>() ? this.container.GetInstance<T>() : this.container.TryGetInstance<T>();
        }

        public static bool IsConcrete<T>()
        {
            return !(typeof(T).IsAbstract || typeof(T).IsInterface);
        }

        public void Dispose()
        {
            this.container.Dispose();
        }
    }
}