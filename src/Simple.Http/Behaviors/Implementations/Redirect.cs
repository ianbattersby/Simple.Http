// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Redirect.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using Behaviors;
    using Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class Redirect
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>false</c> (to prevent response output) if the status is a redirect code; otherwise, <c>true</c>.</returns>
        public static bool Impl(IMayRedirect handler, IContext context)
        {
            var code = context.Response.Status.Code;

            if ((code < 301 || code > 303) && code != 307)
            {
                return true;
            }

            context.Response.SetHeader("Location", handler.Location);

            return false;
        }
    }
}