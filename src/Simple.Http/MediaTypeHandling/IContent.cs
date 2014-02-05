// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContent.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents content to be returned to the client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.MediaTypeHandling
{
    using System;
    using System.Collections.Generic;
    using Simple.Http.Links;

    /// <summary>
    /// Represents content to be returned to the client.
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// Gets the URI used to request the resource.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Gets the handler which generated the model.
        /// </summary>
        object Handler { get; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        object Model { get; }

        /// <summary>
        /// Gets the variables from the handler.
        /// </summary>
        IEnumerable<KeyValuePair<string, object>> Variables { get; }

        /// <summary>
        /// Gets the links which are valid for the model type, based on the <see cref="LinksFromAttribute"/> on handlers.
        /// </summary>
        IEnumerable<Link> Links { get; } 
    }
}