// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISimpleContainer.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Interface to implement for Dependency Injection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.DependencyInjection
{
    using System;

    /// <summary>
    /// Interface to implement for Dependency Injection.
    /// </summary>
    /// <remarks>Wrap this around your favorite IoC container.</remarks>
    public interface ISimpleContainer
    {
        /// <summary>
        /// Begins a nested container / child container / activation block scope
        /// </summary>
        /// <returns>A container that you should dispose to end the block scope.</returns>
        ISimpleContainerScope BeginScope();
    }

    /// <summary>
    /// Interface to implement for scoped objects created by IoC containers.
    /// </summary>
    public interface ISimpleContainerScope : IDisposable
    {
        /// <summary>
        /// Gets an instance of <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The type of thing to construct.</typeparam>
        /// <returns>A new instance of T.</returns>
        T Get<T>();
    }
}