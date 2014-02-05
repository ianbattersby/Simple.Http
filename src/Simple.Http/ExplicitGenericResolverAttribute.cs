// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplicitGenericResolverAttribute.cs" company="Mark Rendle and Ian Battersby.">
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
    using System.Linq;

    /// <summary>
    /// Provides a list of Generic URI types from a regular expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ExplicitGenericResolverAttribute : GenericResolverAttribute
    {
        private readonly Type[] types;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexGenericResolverAttribute" /> class.
        /// </summary>
        /// <param name="uriTemplateName">Name of the URI template part.</param>
        /// <param name="types">All the types for which the UriTemplate is valid.</param>
        public ExplicitGenericResolverAttribute(string uriTemplateName, params Type[] types)
            : base(uriTemplateName)
        {
            this.types = types;
        }

        /// <summary>
        /// When implemented in a derived class, should return a list of all valid types for the generic parameter.
        /// </summary>
        /// <returns>
        /// A list of valid types.
        /// </returns>
        public override IEnumerable<Type> GetTypes()
        {
            return this.types.AsEnumerable();
        }
    }
}