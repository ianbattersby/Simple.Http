// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Link.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents a link, to be sent as part of a resource in a RESTful API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Links
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a link, to be sent as part of a resource in a RESTful API.
    /// </summary>
    [DataContract(Name = "link")]
    public class Link
    {
        private readonly Type handlerType;
        private readonly string href;
        private readonly string rel;

        private readonly string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="handlerType">Type of the handler the link points to.</param>
        /// <param name="href">The href: the URI pointing to the resource/handler.</param>
        /// <param name="rel">The rel: the relationship of the link to the resource it is included with.</param>
        /// <param name="type">The type: the Content-Type returned by the link.</param>
        /// <param name="title">The title: a human-readable name for the link.</param>
        public Link(Type handlerType, string href, string rel, string type, string title)
        {
            this.handlerType = handlerType;
            this.href = href;
            this.rel = rel;
            this.Type = type;
            this.title = title;
        }

        /// <summary>
        /// Gets the title: a human-readable name for the link.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title
        {
            get { return this.title; }
            // ReSharper disable ValueParameterNotUsed (required by DataContractSerializer)
            private set { }
            // ReSharper restore ValueParameterNotUsed
        }

        /// <summary>
        /// Gets the href: the URI pointing to the resource/handler.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href
        {
            get { return this.href; }
            // ReSharper disable ValueParameterNotUsed (required by DataContractSerializer)
            private set { }
            // ReSharper restore ValueParameterNotUsed
        }

        /// <summary>
        /// Gets the rel: the relationship of the link to the resource it is included with.
        /// </summary>
        [DataMember(Name = "rel")]
        public string Rel
        {
            get { return this.rel; }
            // ReSharper disable ValueParameterNotUsed (required by DataContractSerializer)
            private set { }
            // ReSharper restore ValueParameterNotUsed
        }

        /// <summary>
        /// Gets the type: the Content-Type returned by the link.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        /// <returns>The type of the handler.</returns>
        public Type GetHandlerType()
        {
            return this.handlerType;
        }
    }
}