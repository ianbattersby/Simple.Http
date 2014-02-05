// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaType.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Constants for common hypermedia content types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.MediaTypeHandling
{
    /// <summary>
    /// Constants for common hypermedia content types
    /// </summary>
    public static class MediaType
    {
        /// <summary>
        /// Json Mime Type
        /// </summary>
        public const string Json = "application/json";

        /// <summary>
        /// Html Mime Type
        /// </summary>
        public const string Html = "text/html";
        
        /// <summary>
        /// XHtml Mime Type
        /// </summary>
        public const string XHtml = "application/xhtml+xml";
        
        /// <summary>
        /// XML Mime Type
        /// </summary>
        public const string Xml = "application/xml";
    }
}