// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawHtml.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Wraps a string and tells the framework that it should be treated as raw HTML.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    /// <summary>
    /// Wraps a string and tells the framework that it should be treated as raw HTML.
    /// </summary>
    /// <remarks>This class is instantiated by implicit casting from <see cref="string"/> instances.</remarks>
    public class RawHtml
    {
        private readonly string html;

        internal RawHtml(string html)
        {
            this.html = html;
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        public string Html
        {
            get { return this.html; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.html;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Simple.Http.RawHtml"/>.
        /// </summary>
        /// <param name="html">The HTML string.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator RawHtml(string html)
        {
            return new RawHtml(html);
        }
    }
}