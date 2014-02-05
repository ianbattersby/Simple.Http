// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegexGenericResolverAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides a list of Generic URI types from a regular expression.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Simple.Http.Helpers;

    /// <summary>
    /// Provides a list of Generic URI types from a regular expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class RegexGenericResolverAttribute : GenericResolverAttribute
    {
        private readonly Regex regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexGenericResolverAttribute" /> class.
        /// </summary>
        /// <param name="uriTemplateName">Name of the URI template part.</param>
        /// <param name="regex">A regular expression to check all exported types' full names.</param>
        public RegexGenericResolverAttribute(string uriTemplateName, string regex) : base(uriTemplateName)
        {
            this.regex = new Regex(regex);
        }

        /// <summary>
        /// When implemented in a derived class, should return a list of all valid types for the generic parameter.
        /// </summary>
        /// <returns>
        /// A list of valid types.
        /// </returns>
        public override IEnumerable<Type> GetTypes()
        {
            return ExportedTypeHelper.FromCurrentAppDomain(t => this.regex.IsMatch(t.FullName));
        }
    }
}