// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGet.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents a synchronous handler for a GET operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using Protocol;

    /// <summary>
    /// Represents a synchronous handler for a GET operation.
    /// </summary>
    [HttpMethod("GET")]
    public interface IGet
    {
        /// <summary>
        /// The entry point for the Handler
        /// </summary>
        /// <returns>A <see cref="Status"/> representing the status of the operation.</returns>
        /// <remarks>You can also return an <see cref="int"/> from this method, as long as it is a valid HTTP Status Code.</remarks>
        Status Get();
    }
}