// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContext.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents the context for a request/response cycle.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Protocol
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the context for a request/response cycle.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Gets the request.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        /// Gets the response.
        /// </summary>
        IResponse Response { get; }

        /// <summary>
        /// Gets a general-purpose store for variables that can be used for storing stuff for the lifetime of the request.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        IDictionary<string, object> Variables { get; }
    }
}