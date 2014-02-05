// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponse.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Abstraction for an HTTP response, to be implemented by hosting.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstraction for an HTTP response, to be implemented by hosting.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets or sets the status code and description.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        Status Status { get; set; }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        Func<Stream, Task> WriteFunction { get; set; }

        /// <summary>
        /// The response headers.
        /// </summary>
        IDictionary<string, string[]> Headers { get; set; }
    }
}