﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupTaskRunner.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Runs startup tasks (in types with the <see cref="IStartupTask" /> interface). Should be called from the Hosting system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Hosting
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Simple.Http.Helpers;

    /// <summary>
    /// Runs startup tasks (in types with the <see cref="IStartupTask"/> interface). Should be called from the Hosting system.
    /// </summary>
    public sealed class StartupTaskRunner
    {
        private static readonly Type StartupTaskType = typeof(IStartupTask);

        /// <summary>
        /// Runs the startup tasks.
        /// </summary>
        public void RunStartupTasks()
        {
            foreach (var type in ExportedTypeHelper.FromCurrentAppDomain(StartupTaskType.IsAssignableFrom))
            {
                if (!(type.IsInterface || type.IsAbstract))
                {
                    CreateAndRunTask(type);
                }
            }
        }

        private static void CreateAndRunTask(Type type)
        {
            try
            {
                TryRun(type);
            }
            catch (TargetInvocationException ex)
            {
                Trace.TraceError(ex.Message);
            }
            catch (MethodAccessException ex)
            {
                Trace.TraceError(ex.Message);
            }
            catch (MemberAccessException ex)
            {
                Trace.TraceError(ex.Message);
            }
            catch (TypeLoadException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        private static void TryRun(Type type)
        {
            var task = Activator.CreateInstance(type) as IStartupTask;
            if (task != null)
            {
                task.Run(SimpleHttp.Configuration, SimpleHttp.Environment);
            }
        }
    }
}