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
            var container = Expression.Constant(this.configuration.Container);
            var containerScoped = Expression.Call(container, this.configuration.Container.GetType().GetMethod("BeginScope"));
            var scope = Expression.Variable(typeof(ISimpleContainerScope));

            // Create handler block
            var getMethod = Expression.Call(scope, typeof(ISimpleContainerScope).GetMethod("Get").MakeGenericMethod(type));
            var instance = Expression.Variable(type);
            var construct = Expression.Assign(instance, getMethod);
            var variables = Expression.Parameter(typeof(IDictionary<string, string>));
            var handlerBlock = PropertySetterBuilder.MakePropertySetterBlock(type, variables, instance, construct);

            // Wrap handler block in IScopedHandler so we can dispose it later
            var createdInstance = Expression.Variable(type);
            var scopedHandler = Expression.Variable(typeof(IScopedHandler));

            var lines = new List<Expression> 
            {
                Expression.Assign(scope, containerScoped),
                Expression.Assign(createdInstance, handlerBlock),
                scopedHandler,
                Expression.Assign(scopedHandler, Expression.Call(typeof(ScopedHandler).GetMethod("Create", BindingFlags.Static | BindingFlags.Public), createdInstance, scope)),
                scopedHandler
            };

            var scopeBlock = Expression.Block(typeof(IScopedHandler), new[] { createdInstance, scope, scopedHandler }, lines);

            return Expression.Lambda<Func<IDictionary<string, string>, IScopedHandler>>(scopeBlock, variables).Compile();
        }
    }
}