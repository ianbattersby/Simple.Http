// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkAttributeBase.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Base class for <see cref="CanonicalAttribute" /> and <see cref="LinksFromAttribute" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Links
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base class for <see cref="CanonicalAttribute"/> and <see cref="LinksFromAttribute"/>.
    /// </summary>
    public abstract class LinkAttributeBase : Attribute
    {
        private readonly Type modelType;
        private readonly string uriTemplate;

        protected LinkAttributeBase(Type modelType, string uriTemplate)
        {
            this.modelType = modelType;
            this.uriTemplate = uriTemplate;
        }

        /// <summary>
        /// Gets the URI template.
        /// </summary>
        public string UriTemplate
        {
            get { return this.uriTemplate; }
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType
        {
            get { return this.modelType; }
        }

        /// <summary>
        /// Gets or sets the Content-Type of the resource.
        /// </summary>
        /// <value>
        /// The Content-Type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the title: a human-readable name for the link.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets the rel: the relationship of the linked resource to the current one.
        /// </summary>
        /// <returns>The <c>rel</c> type, e.g. <c>self</c> for canonical links.</returns>
        internal abstract string GetRel();

        /// <summary>
        /// Checks to see if the attribute exists on a type.
        /// </summary>
        /// <param name="type">The handler.</param>
        /// <returns><c>true</c> if the attribute is applied to the type; otherwise, <c>false</c>.</returns>
        public static bool Exists(Type type)
        {
            return Attribute.IsDefined(type, typeof(LinkAttributeBase));
        }

        /// <summary>
        /// Gets a list of this type of attribute for a handler type and a model type.
        /// </summary>
        /// <param name="handlerType">The handler type.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>A list of <see cref="CanonicalAttribute"/> or <see cref="LinksFromAttribute"/> objects.</returns>
        public static IList<LinkAttributeBase> Get(Type handlerType, Type modelType)
        {
            return Get(handlerType)
                .Where(a => a.ModelType != null && a.ModelType.IsAssignableFrom(modelType))
                .ToList();
        }

        public static IEnumerable<LinkAttributeBase> Get(Type handlerType)
        {
            return GetCustomAttributes(handlerType, typeof(LinkAttributeBase))
                .Cast<LinkAttributeBase>();
        }
    }
}