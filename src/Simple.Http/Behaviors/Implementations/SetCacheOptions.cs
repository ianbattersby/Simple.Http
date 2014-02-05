// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetCacheOptions.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetCacheOptions
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        public static void Impl(ICacheability handler, IContext context)
        {
            CacheOptions cacheOptions;

            if ((cacheOptions = handler.CacheOptions) == null)
            {
                return;
            }

            context.Response.SetCacheOptions(cacheOptions);
        }
    }
}