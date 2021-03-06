// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPost.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents a synchronous handler for a POST operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using Protocol;

    /// <summary>
    /// Represents a synchronous handler for a POST operation.
    /// </summary>
    [HttpMethod("POST")]
    public interface IPost
    {
        /// <summary>
        /// The entry point for the Handler
        /// </summary>
        /// <returns>A <see cref="Status"/> representing the status of the operation.</returns>
        /// <remarks>You can also return an <see cref="int"/> from this method, as long as it is a valid HTTP Status Code.</remarks>
        Status Post();
    }

    /// <summary>
    /// Represents a synchronous handler for a POST operation where the input model is passes as a parameter.
    /// </summary>
    /// <typeparam name="T">The type of the input model</typeparam>
    [HttpMethod("POST")]
    public interface IPost<in T>
    {
        /// <summary>
        /// The entry point for the Handler.
        /// </summary>
        /// <param name="input">The input model, deserialized from the Request stream.</param>
        /// <returns>A <see cref="Status"/> representing the status of the operation.</returns>
        /// <remarks>You can also return an <see cref="int"/> from this method, as long as it is a valid HTTP Status Code.</remarks>
        Status Post(T input);
    }
}