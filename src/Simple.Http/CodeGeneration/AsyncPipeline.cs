// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncPipeline.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the AsyncPipeline type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using Simple.Http;
    using Simple.Http.OwinSupport;
    using Simple.Http.Protocol;

    internal class AsyncPipeline
    {
        public static readonly MethodInfo DefaultStartMethod = typeof(AsyncPipeline).GetMethod(
            "DefaultStart",
            BindingFlags.Static | BindingFlags.NonPublic);

        public static MethodInfo StartMethod(Type handlerType)
        {
            return GetMethod("Start", handlerType);
        }

        public static MethodInfo ContinueWithHandlerMethod(Type handlerType)
        {
            return GetMethod("ContinueWithHandler", handlerType);
        }

        public static MethodInfo ContinueWithAsyncHandlerMethod(Type handlerType)
        {
            return GetMethod("ContinueWithAsyncHandler", handlerType);
        }

        public static MethodInfo Block(Type handlerType)
        {
            return GetMethod("Block", handlerType);
        }

        public static MethodInfo ContinueWithAsyncBlockMethod(Type handlerType)
        {
            return GetMethod("ContinueWithAsyncBlock", handlerType);
        }

        public static MethodInfo ContinueWithActionMethod(Type handlerType)
        {
            return GetMethod("ContinueWithAction", handlerType);
        }

        private static MethodInfo GetMethod(string name, Type handlerType)
        {
            return typeof(AsyncPipeline).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(handlerType);
        }

        private static Task<bool> DefaultStart()
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }

        private static Task<bool> Start<THandler>(Func<THandler, IContext, Task<bool>> func, IContext context, THandler handler)
        {
            return func(handler, context);
        }

        private static bool Block<THandler>(
            TaskCompletionSource<bool> tcs,
            THandler handler,
            IContext context)
        {
            return false;
        }

        private static bool Handler<THandler>(
            TaskCompletionSource<bool> tcs,
            THandler handler,
            IContext context)
        {
            return false;
        }

        private static Task<bool> ContinueWithAsyncBlock<THandler>(Task<bool> task, Func<THandler, IContext, Task<bool>> continuation, IContext context, THandler handler)
        {
            return task.ContinueWith(
                t =>
                {
                    if (t.Result)
                    {
                        return continuation(handler, context);
                    }

                    return TaskHelper.Completed(false);
                },
                TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap();
        }

        private static Task<bool> ContinueWithHandler<THandler>(Task<bool> task, Func<THandler, IContext, Status> continuation, IContext context, THandler handler)
        {
            return task.ContinueWith(
                t =>
                {
                    if (t.Result)
                    {
                        context.Response.Status = continuation(handler, context);
                        return TaskHelper.Completed(true);
                    }

                    return TaskHelper.Completed(false);
                },
                TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap();
        }

        private static Task<bool> ContinueWithAction<THandler>(
            Task<bool> task,
            Action<THandler, IContext> continuation,
            IContext context,
            THandler handler)
        {
            return task.ContinueWith(
                t =>
                {
                    if (t.Result)
                    {
                        continuation(handler, context);
                        return TaskHelper.Completed(true);
                    }

                    return TaskHelper.Completed(false);
                },
                TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap();
        }

        private static Task WriteResponse(OwinContext context, IDictionary<string, object> env)
        {
            var tcs = new TaskCompletionSource<int>();

            var cancellationToken = (CancellationToken)env[OwinKeys.CallCancelled];

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
            }
            else
            {
                try
                {
                    context.Response.EnsureContentTypeCharset();

                    env.Add(OwinKeys.StatusCode, context.Response.Status.Code);
                    env.Add(OwinKeys.ReasonPhrase, context.Response.Status.Description);

                    if (context.Response.Headers != null)
                    {
                        var responseHeaders = (IDictionary<string, string[]>)env[OwinKeys.ResponseHeaders];

                        foreach (var header in context.Response.Headers)
                        {
                            if (responseHeaders.ContainsKey(header.Key))
                            {
                                responseHeaders[header.Key] = header.Value;
                            }
                            else
                            {
                                responseHeaders.Add(header.Key, header.Value);
                            }
                        }
                    }

                    if (context.Response.WriteFunction != null)
                    {
                        return context.Response.WriteFunction((Stream)env[OwinKeys.ResponseBody]);
                    }

                    tcs.SetResult(0);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            return tcs.Task;
        }
    }
}