// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlerBlock.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the HandlerBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Simple.Http.Behaviors.Implementations;
    using Simple.Http.Protocol;

    internal class HandlerBlock
    {
        private readonly Type handlerType;
        private readonly MethodInfo method;

        public HandlerBlock(Type handlerType, MethodInfo method)
        {
            this.handlerType = handlerType;
            this.method = method;
        }

        public LambdaExpression Generate()
        {
            var paramContext = Expression.Parameter(typeof(IContext), "context");
            var paramHandler = Expression.Parameter(this.handlerType, "handler");

            var parameters = this.method.GetParameters();

            if (parameters.Length == 0)
            {
                Expression call = Expression.Call(paramHandler, this.method);
                return Expression.Lambda(call, paramHandler, paramContext);
            }
            else if (parameters.Length == 1)
            {
                var getInput =
                    typeof(GetInput).GetMethod("Impl", BindingFlags.Public | BindingFlags.Static)
                        .MakeGenericMethod(parameters[0].ParameterType);

                Expression call = Expression.Call(paramHandler, this.method, Expression.Call(getInput, paramContext));

                return Expression.Lambda(call, paramHandler, paramContext);
            }
            else
            {
                throw new InvalidOperationException("Handler methods may only take 0 or 1 parameters.");
            }
        }
    }
}