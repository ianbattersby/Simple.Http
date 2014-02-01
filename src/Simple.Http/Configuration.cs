namespace Simple.Http
{
    using System;
    using System.Collections.Generic;
    using DependencyInjection;
    using MediaTypeHandling;

    /// <summary>
    ///     Default implementation of <see cref="IConfiguration" />.
    /// </summary>
    public sealed class Configuration : IConfiguration
    {
        private ISimpleContainer _container = new DefaultSimpleContainer();

        /// <summary>
        ///     Gets or sets the IoC container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public ISimpleContainer Container
        {
            get { return _container; }
            set { _container = value ?? new DefaultSimpleContainer(); }
        }

        public IMediaTypeHandler DefaultMediaTypeHandler { get; set; }
    }
}