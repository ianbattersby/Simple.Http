// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseExtensions.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Extension methods for <see cref="IResponse" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System.Text;

    using Simple.Http.Protocol;

    /// <summary>
    /// Extension methods for <see cref="IResponse"/>.
    /// </summary>
    public static class ResponseExtensions
    {
        /// <summary>
        /// Writes text to the response body.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="text">The text to write.</param>
        public static void Write(this IResponse response, string text)
        {
            response.WriteFunction = stream =>
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    return stream.WriteAsync(bytes, 0, bytes.Length);
                };
        }
    }
}