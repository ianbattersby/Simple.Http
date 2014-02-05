// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetFiles.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using System.Linq;
    using Behaviors;
    using Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetFiles
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        public static void Impl(IUploadFiles handler, IContext context)
        {
            if (context.Request.Files.Any())
            {
                handler.Files = context.Request.Files;
            }
        }
    }
}