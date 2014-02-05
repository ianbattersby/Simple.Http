// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHead.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents a synchronous handler for a HEAD operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using Protocol;

    /// <summary>
    /// Represents a synchronous handler for a HEAD operation.
    /// </summary>
    [HttpMethod("HEAD")]
    public interface IHead
    {
        /// <summary>
        /// The entry point for the Handler
        /// </summary>
        /// <returns>A <see cref="Status"/> representing the status of the operation.</returns>
        /// <remarks>You can also return an <see cref="int"/> from this method, as long as it is a valid HTTP Status Code.</remarks>
        Status Head();
    }
}