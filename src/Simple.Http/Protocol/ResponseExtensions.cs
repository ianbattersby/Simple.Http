// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseExtensions.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Extension methods for the <see cref="IResponse" /> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extension methods for the <see cref="IResponse"/> interface.
    /// </summary>
    public static class ResponseExtensions
    {
        private static readonly Regex CharsetCheck = new Regex(@";\s*charset=");

        /// <summary>
        /// Sets the response Content-Type header.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="contentType">The content type. This should be a valid media type.</param>
        public static void SetContentType(this IResponse response, string contentType)
        {
            response.SetHeader(HeaderKeys.ContentType, contentType);
        }

        public static void EnsureContentTypeCharset(this IResponse response, string charset = "utf-8")
        {
            string value;

            if (response.TryGetHeader(HeaderKeys.ContentType, out value))
            {
                if (!CharsetCheck.IsMatch(value))
                {
                    response.SetHeader(HeaderKeys.ContentType, value + "; charset=" + charset);
                }
            }
        }

        /// <summary>
        /// Sets the response Content-Length header.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="contentLength">The content length.</param>
        public static void SetContentLength(this IResponse response, long contentLength)
        {
            response.SetHeader(HeaderKeys.ContentLength, contentLength.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the response Last-Modified header.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="time">The time the resource was last modified.</param>
        public static void SetLastModified(this IResponse response, DateTimeOffset time)
        {
            response.SetHeader(HeaderKeys.LastModified, time.ToString("r"));
        }

        /// <summary>
        /// Sets a response header. Any current values for the specified header field are replaced.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="header">The header key.</param>
        /// <param name="value">The header value.</param>
        public static void SetHeader(this IResponse response, string header, string value)
        {
            EnsureHeaders(response);
            response.Headers[header] = new[] { value };
        }

        /// <summary>
        /// Gets a response header if it is set.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="header">The header key.</param>
        /// <param name="value">The header value if it is set; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the header is set; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the header is set but has multiple values.</exception>
        public static bool TryGetHeader(this IResponse response, string header, out string value)
        {
            if (response.Headers == null || response.Headers.Count == 0)
            {
                value = null;
                return false;
            }

            string[] values;

            if ((!response.Headers.TryGetValue(header, out values)) || values.Length == 0)
            {
                value = null;
                return false;
            }

            if (values.Length == 1)
            {
                value = values[0];
                return true;
            }

            throw new InvalidOperationException("Header has more than one value.");
        }

        /// <summary>
        /// Adds a response header. Current values for the specified header field are retained.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="header">The header key.</param>
        /// <param name="value">The header value.</param>
        public static void AddHeader(this IResponse response, string header, string value)
        {
            EnsureHeaders(response);

            string[] currentValues;

            if (response.Headers.TryGetValue(header, out currentValues))
            {
                Array.Resize(ref currentValues, currentValues.Length + 1);
                currentValues[currentValues.Length - 1] = value;
            }
            else
            {
                currentValues = new[] { value };
            }

            response.Headers[header] = currentValues;
        }

        /// <summary>
        /// Sets the ETag header.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="etag">The ETag value.</param>
        public static void SetETag(this IResponse response, string etag)
        {
            response.SetHeader(HeaderKeys.ETag, etag);
        }

        /// <summary>
        /// Sets the Last-Modified header.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="dateTime">The time the resource was last modified.</param>
        public static void SetLastModified(this IResponse response, DateTime dateTime)
        {
            response.SetHeader(HeaderKeys.LastModified, dateTime.ToUniversalTime().ToString("R"));
        }

        /// <summary>
        /// Disables response caching by setting the Cache-Control header to &quot;no-cache&amp; no-store&quot;.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        public static void DisableCache(this IResponse response)
        {
            response.SetHeader(HeaderKeys.CacheControl, "no-cache; no-store");
        }

        /// <summary>
        /// Sets the Cache-Control header and optionally the Expires and Vary headers.
        /// </summary>
        /// <param name="response">The <see cref="IResponse"/> instance.</param>
        /// <param name="cacheOptions">A <see cref="CacheOptions"/> object to specify the cache settings.</param>
        public static void SetCacheOptions(this IResponse response, CacheOptions cacheOptions)
        {
            if (cacheOptions == null)
            {
                return;
            }

            if (cacheOptions.Disable)
            {
                response.DisableCache();
                return;
            }

            response.SetHeader(HeaderKeys.CacheControl, cacheOptions.ToHeaderString());

            if (cacheOptions.AbsoluteExpiry.HasValue)
            {
                response.SetHeader(HeaderKeys.Expires, cacheOptions.AbsoluteExpiry.Value.ToString("R"));
            }

            if (cacheOptions.VaryByHeaders != null && cacheOptions.VaryByHeaders.Count > 0)
            {
                response.SetHeader(HeaderKeys.Vary, string.Join(", ", cacheOptions.VaryByHeaders));
            }
        }

        private static void EnsureHeaders(IResponse response)
        {
            if (response.Headers == null)
            {
                response.Headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}