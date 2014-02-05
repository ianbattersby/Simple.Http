// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSimpleContainer.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the DefaultSimpleContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.DependencyInjection
{
    internal class DefaultSimpleContainer : ISimpleContainer
    {
        public ISimpleContainerScope BeginScope()
        {
            return new DefaultSimpleContainerScope();
        }
    }
}