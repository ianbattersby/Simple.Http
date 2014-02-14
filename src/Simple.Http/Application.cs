// --------------------------------------------------------------------------------------------------------------------
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
    using System.Linq;
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

            IDictionary<string, string> variables;

            var routingTable = RoutingTables.GetOrAdd(context.Request.HttpMethod, BuildRoutingTable);
            var handlerType = routingTable.GetHandlerTypeForUrl(context.Request.Url.AbsolutePath, out variables);

            if (handlerType == null)
            {
                env.Add(OwinKeys.StatusCode, Status.NotFound.Code);
                env.Add(OwinKeys.ReasonPhrase, Status.NotFound.Description);

                return TaskHelper.Completed(new Result(null, Status.NotFound.Code, null, null));
            }

            RunStartupTasks();

            var handlerInfo = new HandlerInfo(
                handlerType,
                variables,
                context.Request.HttpMethod);

            var handler = PipelineFunctionFactory.Get(handlerInfo.HandlerType, handlerInfo.HttpMethod);
            
            return handler(context, handlerInfo);
       }

        private static void RunStartupTasks()
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
    }
}