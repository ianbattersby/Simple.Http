// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheLevel.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents the HTTP cacheability levels.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Protocol
{
    /// <summary>
    /// Represents the HTTP cacheability levels.
    /// </summary>
    public enum CacheLevel
    {
        /// <summary>
        /// No caching; sets the Cache-Control header to &quot;no-cache&quot;
        /// </summary>
        None,

        /// <summary>
        /// Public caching (allows caching by client and proxies); sets the Cache-Control header to &quot;public&quot;
        /// </summary>
        Public,
        
        /// <summary>
        /// Private caching (allows caching by server only); sets the Cache-Control header to &quot;private&quot;
        /// </summary>
        Private
    }
}