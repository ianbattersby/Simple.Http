﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Threading;
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
            var context = Expression.Parameter(typeof(IContext));
            var handler = Expression.Parameter(handlerType);

            var calls = new List<Expression>();
            calls.AddRange(this.methods.Select(m => CreateCall(m, handler, context, handlerType)));

            if (this.methods.Last().ReturnType == typeof(void))
            {
                calls.Add(Expression.Call(typeof(PipelineBlock).GetMethod("CompletedTask", BindingFlags.Static | BindingFlags.NonPublic)));
            }
            else if (this.methods.Last().ReturnType == typeof(bool))
            {
                FixLastCall(calls, "CompleteTask");
            }
            else if (this.methods.Last().ReturnType == typeof(Task))
            {
                FixLastCall(calls, "CompleteTask");
            }
            else if (this.methods.Last().ReturnType == typeof(Task<bool>))
            {
                FixLastCall(calls, "CancelBooleanAsync");
            }
            else
            {
                throw new InvalidOperationException(
                    "Behavior implementation methods may only return void, bool, Task, or Task<bool>.");
            }

            var block = Expression.Block(calls);

            return Expression.Lambda(block, handler, context).Compile();
        }

        private static Expression CreateCall(MethodInfo method, ParameterExpression handler, ParameterExpression context, Type handlerType)
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

        private static Task<bool> CompletedTask(bool @continue = true)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(@continue);
            return tcs.Task;
        }

        private static Task<bool> CompleteTask(Task task)
        {
            return task.ContinueWith(t => true, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static Task<bool> CancelBooleanAsync(Task<bool> task)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            return task.ContinueWith(
                t =>
                {
                    if (!t.Result)
                    {
                        cancellationTokenSource.Cancel();
                    }

                    return t.Result;
                },
                cancellationTokenSource.Token);
        }
    }
}