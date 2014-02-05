// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleHttp.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Configuration and Environment information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    /// <summary>
    /// Configuration and Environment information.
    /// </summary>
    public static class SimpleHttp
    {
        /// <summary>
        /// The current Configuration for the app.
        /// </summary>
        public static readonly IConfiguration Configuration = new Configuration();

        /// <summary>
        /// Environmental information for the app.
        /// </summary>
        public static readonly IWebEnvironment Environment = new WebEnvironment();
    }
}