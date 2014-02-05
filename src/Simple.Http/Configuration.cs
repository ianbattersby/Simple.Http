// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Default implementation of <see cref="IConfiguration" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http
{
    using DependencyInjection;
    using MediaTypeHandling;

    /// <summary>
    ///     Default implementation of <see cref="IConfiguration" />.
    /// </summary>
    public sealed class Configuration : IConfiguration
    {
        private ISimpleContainer container = new DefaultSimpleContainer();

        /// <summary>
        ///     Gets or sets the IoC container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public ISimpleContainer Container
        {
            get { return this.container; }
            set { this.container = value ?? new DefaultSimpleContainer(); }
        }

        public IMediaTypeHandler DefaultMediaTypeHandler { get; set; }
    }
}