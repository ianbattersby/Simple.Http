// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapContainer.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the StructureMapContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.StructureMap
{
    using Simple.Http.DependencyInjection;

    using global::StructureMap;

    public class StructureMapContainer : ISimpleContainer
    {
        private readonly IContainer container;

        internal StructureMapContainer(IContainer container)
        {
            this.container = container;
        }

        public ISimpleContainerScope BeginScope()
        {
            return new StructureMapContainerScope(this.container.GetNestedContainer());
        }
    }
}
