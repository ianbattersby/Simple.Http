// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Redirect2.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class Redirect2
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>false</c> (to prevent response output) if the status is a redirect code; otherwise, <c>true</c>.</returns>
        public static bool Impl(object handler, IContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Response.Status.LocationHeader))
            {
                return true;
            }

            context.Response.SetHeader("Location", context.Response.Status.LocationHeader);

            return false;
        }
    }
}