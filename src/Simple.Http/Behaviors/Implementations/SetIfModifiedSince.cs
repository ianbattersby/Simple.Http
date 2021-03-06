// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetIfModifiedSince.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using System;
    using System.Linq;
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetIfModifiedSince
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        public static void Impl(IModified handler, IContext context)
        {
            if (!context.Request.Headers.ContainsKey("If-Modified-Since"))
            {
                return;
            }

            var header = context.Request.Headers["If-Modified-Since"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(header))
            {
                DateTime time;

                if (DateTime.TryParse(header, out time))
                {
                    handler.IfModifiedSince = time;
                }
            }
        }
    }
}