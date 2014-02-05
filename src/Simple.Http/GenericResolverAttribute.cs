// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericResolverAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Base class for attributes that resolve Generic UriTemplate parameters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for attributes that resolve Generic UriTemplate parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public abstract class GenericResolverAttribute : Attribute
    {
        private readonly string uriTemplateName;

        protected GenericResolverAttribute(string uriTemplateName)
        {
            this.uriTemplateName = uriTemplateName;
        }

        /// <summary>
        /// The name of the type in the UriTemplate.
        /// </summary>
        public string UriTemplateName
        {
            get { return this.uriTemplateName; }
        }

        /// <summary>
        /// When implemented in a derived class, should return a list of all valid types for the generic parameter.
        /// </summary>
        /// <returns>A list of valid types.</returns>
        public abstract IEnumerable<Type> GetTypes();

        /// <summary>
        /// Returns a list of string names that are valid in a URI for the given type.
        /// </summary>
        /// <param name="type">The type from which to derive the valid names.</param>
        /// <returns>A list of names.</returns>
        public virtual IEnumerable<string> GetNames(Type type)
        {
            yield return type.Name;
        }
    }
}