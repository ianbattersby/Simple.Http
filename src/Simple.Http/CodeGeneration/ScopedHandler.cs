// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScopedHandler.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Default implementation of <see cref="IScopedHandler" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using DependencyInjection;

    /// <summary>
    /// Default implementation of <see cref="IScopedHandler"/>.
    /// </summary>
    public sealed class ScopedHandler : IScopedHandler
    {
        private readonly ISimpleContainerScope scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedHandler" /> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="scope">The scope.</param>
        public ScopedHandler(object handler, ISimpleContainerScope scope)
        {
            this.Handler = handler;
            this.scope = scope;
        }

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <value>
        /// The handler.
        /// </value>
        public object Handler { get; private set; }

        /// <summary>
        /// Creates the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>An <see cref="IScopedHandler"/> wrapped around a handler object.</returns>
        public static IScopedHandler Create(object handler, ISimpleContainerScope scope)
        {
            return new ScopedHandler(handler, scope);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.scope.Dispose();
        }
    }
}