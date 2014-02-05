// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileUtility.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides methods for working with files in an application server.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    /// <summary>
    /// Provides methods for working with files in an application server.
    /// </summary>
    public interface IFileUtility
    {
        /// <summary>
        /// Checks to see whether a path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists; otherwise <c>false</c>.</returns>
        bool Exists(string path);
    }
}