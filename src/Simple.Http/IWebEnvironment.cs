// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWebEnvironment.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides information about the environment for the application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using Helpers;

    /// <summary>
    /// Provides information about the environment for the application.
    /// </summary>
    public interface IWebEnvironment
    {
        /// <summary>
        /// Gets the root folder of the application in the host.
        /// </summary>
        string AppRoot { get; }

        /// <summary>
        /// Gets the path utility.
        /// </summary>
        IPathUtility PathUtility { get; }

        /// <summary>
        /// Gets the file utility.
        /// </summary>
        IFileUtility FileUtility { get; }
    }
}