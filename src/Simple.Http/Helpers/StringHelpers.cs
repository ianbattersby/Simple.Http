// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringHelpers.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Handy extensions for strings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System;

    /// <summary>
    /// Handy extensions for strings.
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Return the substring up to but not including the first instance of 'c'.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The character to look for.</param>
        /// <returns>The substring up to but not including the first instance of 'c'. If 'c' is not found, the entire string is returned.</returns>
        public static string SubstringBefore(this string src, char c)
        {
            if (string.IsNullOrEmpty(src))
            {
                return string.Empty;
            }

            var idx = Math.Min(src.Length, src.IndexOf(c));

            return idx < 0 ? src : src.Substring(0, idx);
        }

        /// <summary>
        /// Return the substring up to but not including the last instance of 'c'.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The character to look for.</param>
        /// <returns>The substring up to but not including the last instance of 'c'. If 'c' is not found, the entire string is returned.</returns>
        public static string SubstringBeforeLast(this string src, char c)
        {
            if (string.IsNullOrEmpty(src))
            {
                return string.Empty;
            }

            var idx = Math.Min(src.Length, src.LastIndexOf(c));

            return idx < 0 ? src : src.Substring(0, idx);
        }

        /// <summary>
        /// Return the substring after but not including the first instance of 'c'.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The character to look for.</param>
        /// <returns>The substring after but not including the first instance of 'c'. If 'c' is not found, the entire string is returned.</returns>
        public static string SubstringAfter(this string src, char c)
        {
            if (string.IsNullOrEmpty(src))
            {
                return string.Empty;
            }

            var idx = Math.Min(src.Length - 1, src.IndexOf(c) + 1);

            return idx < 0 ? src : src.Substring(idx);
        }

        /// <summary>
        /// Return the substring after but not including the last instance of 'c'.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The character to look for.</param>
        /// <returns>The substring after but not including the last instance of 'c'. If 'c' is not found, the entire string is returned.</returns>
        public static string SubstringAfterLast(this string src, char c)
        {
            if (string.IsNullOrEmpty(src))
            {
                return string.Empty;
            }

            var idx = Math.Min(src.Length - 1, src.LastIndexOf(c) + 1);

            return idx < 0 ? src : src.Substring(idx);
        }
    }
}