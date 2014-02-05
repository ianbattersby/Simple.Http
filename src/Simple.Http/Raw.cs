// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Raw.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Static methods for constructing <see cref="RawHtml" /> objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using System.Text;

    /// <summary>
    /// Static methods for constructing <see cref="RawHtml"/> objects.
    /// </summary>
    public static class Raw
    {
        /// <summary>
        /// Creates a <see cref="RawHtml"/> object from a <see cref="string"/>.
        /// </summary>
        /// <param name="html">The HTML string.</param>
        /// <returns>A strongly-typed HTML object.</returns>
        public static RawHtml Html(string html)
        {
            return new RawHtml(html);
        }

        /// <summary>
        /// Creates a <see cref="RawHtml"/> object from a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="html">The HTML string.</param>
        /// <returns>A strongly-typed HTML object.</returns>
        public static RawHtml Html(StringBuilder html)
        {
            return new RawHtml(html.ToString());
        }
    }
}