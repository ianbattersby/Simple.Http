// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Content.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Default implementation of the <see cref="IContent" /> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.MediaTypeHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Simple.Http.Links;

    /// <summary>
    /// Default implementation of the <see cref="IContent"/> interface.
    /// </summary>
    public class Content : IContent
    {
        private readonly Uri uri;
        private readonly object handler;
        private readonly object model;

        public Content(Uri uri, object handler, object model)
        {
            this.uri = uri;
            this.handler = handler;
            this.model = model;
        }

        /// <summary>
        /// Gets the handler which generated the model.
        /// </summary>
        public object Handler
        {
            get { return this.handler; }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        public object Model
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets the variables from the handler.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> Variables
        {
            get
            {
                return
                    this.handler.GetType().GetProperties().Where(p => p.CanRead).Select(
                        p => new KeyValuePair<string, object>(p.Name, p.GetValue(this.handler, null)));
            }
        }

        /// <summary>
        /// Gets the links which are valid for the model type, based on the <see cref="LinksFromAttribute"/> on handlers.
        /// </summary>
        public IEnumerable<Link> Links
        {
            get
            {
                return LinkHelper.GetLinksForModel(this.model);
            }
        }

        /// <summary>
        /// Gets the URI used to retrieve the resource.
        /// </summary>
        public Uri Uri
        {
            get { return this.uri; }
        }
    }
}