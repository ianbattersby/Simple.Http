// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapStartupBase.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the StructureMapStartupBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.StructureMap
{
    using global::StructureMap;

    public abstract class StructureMapStartupBase : IStartupTask
    {
        public void Run(IConfiguration configuration)
        {
            ObjectFactory.Configure(this.Configure);
            configuration.Container = new StructureMapContainer(ObjectFactory.Container);
        }

        internal protected abstract void Configure(ConfigurationExpression cfg);
    }
}