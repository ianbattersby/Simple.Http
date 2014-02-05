// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileUtility.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the FileUtility type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System.IO;

    internal sealed class FileUtility : IFileUtility
    {
        /// <summary>
        /// Checks to see whether a path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        ///   <c>true</c> if the path exists; otherwise <c>false</c>.
        /// </returns>
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}