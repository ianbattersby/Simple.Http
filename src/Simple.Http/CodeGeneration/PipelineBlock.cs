// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PipelineBlock.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the PipelineBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Simple.Http.Protocol;

    internal class PipelineBlock
    {
        private readonly List<MethodInfo> methods = new List<MethodInfo>();

        public bool Any
        {
            get { return this.methods.Count > 0; }
        }

        public bool IsBoolean
        {
            get { return this.methods.Count > 0 && this.methods.Last().ReturnType == typeof(bool); }
        }

        public void Add(MethodInfo method)
        {
            this.methods.Add(method);
        }

        public Delegate Generate(Type handlerType)
        {
            var paramContext = Expression.Parameter(typeof(IContext), "context");
            var paramHandler = Expression.Parameter(handlerType, "handler");

            var calls = new List<Expression>();
            calls.AddRange(this.methods.Select(m => CreateCall(m, paramHandler, paramContext, handlerType)));

            if (this.methods.Last().ReturnType == typeof(void))
            {
                calls.Add(Expression.Call(typeof(PipelineBlock).GetMethod("CompletedTask", BindingFlags.Static | BindingFlags.NonPublic)));
            }
            else if (this.methods.Last().ReturnType == typeof(bool))
            {
                FixLastCall(calls, "CompleteBooleanTask");
            }
            else if (this.methods.Last().ReturnType == typeof(Task))
            {
                FixLastCall(calls, "CompleteAsyncTask");
            }
            else
            {
                throw new InvalidOperationException(
                    "Behavior implementation methods may only return void, bool, Task, or Task<bool>.");
            }

            var block = Expression.Block(calls);

            return Expression.Lambda(block, paramHandler, paramContext).Compile();
        }

        public static Expression CreateCall(MethodInfo method, Expression handler, Expression context, Type handlerType)
        {
            if (!method.IsGenericMethod)
            {
                return Expression.Call(method, handler, context);
            }

            var handlerParameterType = method.GetParameters()[0].ParameterType;

            if (!handlerParameterType.IsGenericType)
            {
                return Expression.Call(method, handler, context);
            }

            var @interface =
                handlerType.GetInterfaces().FirstOrDefault(
                    i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == handlerParameterType.GetGenericTypeDefinition());

            if (@interface != null)
            {
                method = method.MakeGenericMethod(@interface.GetGenericArguments().Single());
            }

            return Expression.Call(method, handler, context);
        }

        private static void FixLastCall(List<Expression> calls, string methodName)
        {
            var lastCall = calls.Last();
            calls.Remove(lastCall);
            calls.Add(Expression.Call(typeof(PipelineBlock).GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic), lastCall));
        }

        private static Task<bool> CompleteBooleanTask(bool @continue = true)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(@continue);
            return tcs.Task;
        }

        private static Task<bool> CompletedTask()
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }
    }
}