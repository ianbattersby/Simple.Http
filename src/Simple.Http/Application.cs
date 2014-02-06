﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Application.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   The running application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CodeGeneration;
    using Helpers;
    using Hosting;
    using Protocol;

    using Routing;

    using Simple.Http.OwinSupport;
#pragma warning disable 811
    using Result = System.Tuple<System.Collections.Generic.IDictionary<string, object>, int, System.Collections.Generic.IDictionary<string, string[]>, System.Func<System.IO.Stream, System.Threading.Tasks.Task>>;
#pragma warning restore 811

    /// <summary>
    /// The running application.
    /// </summary>
    public class Application
    {
        private static readonly object StartupLock = new object();
        private static volatile StartupTaskRunner startupTaskRunner = new StartupTaskRunner();

        /// <summary>
        /// The OWIN standard application method.
        /// </summary>
        /// <param name="env"> Request life-time general variable storage </param>
        /// <returns>A <see cref="Task"/> which will complete the request.</returns>
        [Export("Owin.Application")]
        public static Task Run(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            var task = Run(context);

            if (task == null)
            {
                env.Add(OwinKeys.StatusCode, Status.NotFound.Code);
                env.Add(OwinKeys.ReasonPhrase, Status.NotFound.Description);

                return TaskHelper.Completed(new Result(null, Status.NotFound.Code, null, null));
            }

            return task
                .ContinueWith(
                    t => 
                        WriteResponse(context, env), 
                        TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnFaulted)
                .Unwrap();
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

        internal static Task Run(IContext context)
        {
            Startup();

            IDictionary<string, string> variables;

            var handlerType = TableFor(context.Request.HttpMethod)
                .Get(
                    context.Request.Url.AbsolutePath,
                    out variables,
                    context.Request.GetContentType(),
                    context.Request.GetAccept());

            if (handlerType == null)
            {
                return null;
            }

            var handlerInfo = new HandlerInfo(handlerType, variables, context.Request.HttpMethod);

            foreach (var key in context.Request.QueryString.Keys.Where(k => !string.IsNullOrWhiteSpace(k)))
            {
                handlerInfo.Variables.Add(key, CombineQueryStringValues(context.Request.QueryString[key]));
            }

            var task = PipelineFunctionFactory.Get(handlerInfo.HandlerType, handlerInfo.HttpMethod)(context, handlerInfo);

            return task ?? TaskHelper.Completed();
        }

        private static string CombineQueryStringValues(string[] values)
        {
            return values.Length == 1 ? values[0] : string.Join("\t", values);
        }

        private static void Startup()
        {
            if (startupTaskRunner != null)
            {
                lock (StartupLock)
                {
                    if (startupTaskRunner != null)
                    {
                        startupTaskRunner.RunStartupTasks();
                        startupTaskRunner = null;
                    }
                }
            }
        }

        private static readonly ConcurrentDictionary<string, RoutingTable> RoutingTables = new ConcurrentDictionary<string, RoutingTable>(StringComparer.OrdinalIgnoreCase);

        internal static RoutingTable BuildRoutingTable(string httpMethod)
        {
            var types = ExportedTypeHelper.FromCurrentAppDomain(IsHttpMethodHandler).ToList();
            var handlerTypes = types
                .Where(i => HttpMethodAttribute.Matches(i, httpMethod))
                .ToArray();

            return new RoutingTableBuilder(handlerTypes).BuildRoutingTable();
        }

        private static bool IsHttpMethodHandler(Type type)
        {
            return (!type.IsInterface || type.IsAbstract) && HttpMethodAttribute.IsAppliedTo(type);
        }

        private static RoutingTable TableFor(string httpMethod)
        {
            return RoutingTables.GetOrAdd(httpMethod, BuildRoutingTable);
        }
    }
}