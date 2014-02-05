// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStartupTask.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Implement this interface to run code at application startup, before the first request is run.
//   For example, use a startup task to configure static files and folders, or an IoC container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    /// <summary>
    /// Implement this interface to run code at application startup, before the first request is run.
    /// For example, use a startup task to configure static files and folders, or an IoC container.
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// Runs the startup task. This method is called by the framework.
        /// </summary>
        /// <param name="configuration">The active Simple.Http configuration.</param>
        /// <param name="environment">The active Simple.Http environment.</param>
        void Run(IConfiguration configuration, IWebEnvironment environment);
    }
}