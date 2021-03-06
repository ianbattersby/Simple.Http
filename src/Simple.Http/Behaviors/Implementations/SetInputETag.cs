// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetInputETag.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using System.Linq;
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetInputETag
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        public static void Impl(IETag handler, IContext context)
        {
            if (!context.Request.Headers.ContainsKey("ETag"))
            {
                return;
            }

            var etag = context.Request.Headers["ETag"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(etag))
            {
                handler.InputETag = etag;
            }
        }
    }
}