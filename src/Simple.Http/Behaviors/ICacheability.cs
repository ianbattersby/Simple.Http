// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheability.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Indicates that a handler exposes caching information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors
{
    using Protocol;

    /// <summary>
    /// Indicates that a handler exposes caching information.
    /// </summary>
    [ResponseBehavior(typeof(Implementations.SetCacheOptions))]
    public interface ICacheability
    {
        /// <summary>
        /// Gets the cache options.
        /// </summary>
        CacheOptions CacheOptions { get; }
    }
}