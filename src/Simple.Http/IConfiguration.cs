// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfiguration.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides configuration details for the application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using DependencyInjection;
    using MediaTypeHandling;

    /// <summary>
    /// Provides configuration details for the application.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets or sets the HostedPath which the host is bound to and UriTemplate suffix.
        /// </summary>
        string HostedPath { get; set; }

        /// <summary>
        /// Gets or sets the IoC container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        ISimpleContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the MediaTypeHandler to use when Accept is */*
        /// </summary>
        /// <value>
        /// An <see cref="IMediaTypeHandler"/> instance
        /// </value>
        IMediaTypeHandler DefaultMediaTypeHandler { get; set; }
    }
}