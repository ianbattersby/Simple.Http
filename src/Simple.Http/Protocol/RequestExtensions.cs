// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestExtensions.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Extension methods for the <see cref="IRequest" /> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Protocol
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for the <see cref="IRequest"/> interface.
    /// </summary>
    public static class RequestExtensions
    {
        private static readonly string[] MediaTypeWildcard = { "*/*" };

        /// <summary>
        /// Gets the Accept header entry.
        /// </summary>
        /// <param name="request">The <see cref="IRequest"/> instance.</param>
        /// <returns>The Accept header value(s), or a wildcard if not in the Headers collection.</returns>
        public static IList<string> GetAccept(this IRequest request)
        {
            string[] accept;

            if (request.Headers == null || (!request.Headers.TryGetValue(HeaderKeys.Accept, out accept)) || accept[0].StartsWith("*/*"))
            {
                accept = MediaTypeWildcard;
            }
            else
            {
                accept = accept.SelectMany(line => line.Split(',').Select(s => s.Trim())).ToArray();
            }

            return accept;
        }

        /// <summary>
        /// Gets the Content-Type header entry.
        /// </summary>
        /// <param name="request">The <see cref="IRequest"/> instance.</param>
        /// <returns>The Content-Type header value, or a wildcard if not in the Headers collection.</returns>
        public static string GetContentType(this IRequest request)
        {
            string[] contentType;

            if (request.Headers == null || !request.Headers.TryGetValue(HeaderKeys.ContentType, out contentType))
            {
                return null;
            }

            return contentType.FirstOrDefault();
        }
    }
}