// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScopedHandler.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   A scoped wrapper around a Handler for use by Dependency Injection libraries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;

    /// <summary>
    /// A scoped wrapper around a Handler for use by Dependency Injection libraries.
    /// </summary>
    public interface IScopedHandler : IDisposable
    {
        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <value>
        /// The handler.
        /// </value>
        object Handler { get; }
    }
}