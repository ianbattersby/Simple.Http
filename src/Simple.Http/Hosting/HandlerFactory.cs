// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlerFactory.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Builds handlers. To be used by Hosting plug-ins.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Hosting
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;

    using Simple.Http.CodeGeneration;

    /// <summary>
    /// Builds handlers. To be used by Hosting plug-ins.
    /// </summary>
    internal sealed class HandlerFactory
    {
        public static readonly MethodInfo GetHandlerMethod = typeof(HandlerFactory).GetMethod("GetHandler");

        private static HandlerFactory instance;

        private readonly HandlerBuilderFactory handlerBuilderFactory;

        private readonly
            ConcurrentDictionary<string, ConcurrentDictionary<Type, Func<IDictionary<string, string>, IScopedHandler>>> builders =
                new ConcurrentDictionary<string, ConcurrentDictionary<Type, Func<IDictionary<string, string>, IScopedHandler>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerFactory"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <remarks>For testing only. In production, use the singleton <see cref="Instance"/>.</remarks>
        public HandlerFactory(IConfiguration configuration)
        {
            this.handlerBuilderFactory = new HandlerBuilderFactory(configuration);
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static HandlerFactory Instance
        {
            get { return instance ?? (instance = new HandlerFactory(SimpleHttp.Configuration)); }
        }

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <param name="handlerInfo">The handler info.</param>
        /// <returns>ScopedHandler object.</returns>
        public IScopedHandler GetHandler(HandlerInfo handlerInfo)
        {
            var builderDictionary = this.builders.GetOrAdd(
                handlerInfo.HttpMethod,
                _ => new ConcurrentDictionary<Type, Func<IDictionary<string, string>, IScopedHandler>>());

            var builder = builderDictionary.GetOrAdd(handlerInfo.HandlerType, this.handlerBuilderFactory.BuildHandlerBuilder);
            var handler = builder(handlerInfo.Variables);

            return handler;
        }
    }
}