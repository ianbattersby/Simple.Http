// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlerBuilderFactory.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the HandlerBuilderFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Simple.Http.DependencyInjection;

    internal class HandlerBuilderFactory
    {
        private readonly IConfiguration configuration;

        public HandlerBuilderFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Func<IDictionary<string, string>, IScopedHandler> BuildHandlerBuilder(Type type)
        {
            // Begin container scope
            var constContainer = Expression.Constant(this.configuration.Container);
            var callContainerScoped = Expression.Call(constContainer, this.configuration.Container.GetType().GetMethod("BeginScope"));
            var varScope = Expression.Variable(typeof(ISimpleContainerScope), "scope");

            // Create handler block
            var callGetMethod = Expression.Call(varScope, typeof(ISimpleContainerScope).GetMethod("Get").MakeGenericMethod(type));
            var varInstance = Expression.Variable(type, "handler");
            var assignConstruct = Expression.Assign(varInstance, callGetMethod);
            var paramVariables = Expression.Parameter(typeof(IDictionary<string, string>), "variables");
            var blockHandler = PropertySetterBuilder.MakePropertySetterBlock(type, paramVariables, varInstance, assignConstruct);

            // Wrap handler block in IScopedHandler so we can dispose it later
            var varCreatedInstance = Expression.Variable(type, "handler");
            var varScopedHandler = Expression.Variable(typeof(IScopedHandler), "scopedHandler");

            var lines = new List<Expression> 
            {
                Expression.Assign(varScope, callContainerScoped),
                Expression.Assign(varCreatedInstance, blockHandler),
                varScopedHandler,
                Expression.Assign(varScopedHandler, Expression.Call(typeof(ScopedHandler).GetMethod("Create", BindingFlags.Static | BindingFlags.Public), varCreatedInstance, varScope)),
                varScopedHandler
            };

            var scopeBlock = Expression.Block(typeof(IScopedHandler), new[] { varCreatedInstance, varScope, varScopedHandler }, lines);

            return Expression.Lambda<Func<IDictionary<string, string>, IScopedHandler>>(scopeBlock, paramVariables).Compile();
        }
    }
}