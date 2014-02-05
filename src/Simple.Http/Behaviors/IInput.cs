// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInput.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Represents a handler that is expecting input.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors
{
    /// <summary>
    /// Represents a handler that is expecting input.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    [RequestBehavior(typeof(Implementations.SetInput))]
    public interface IInput<in TInput>
    {
        /// <summary>
        /// Used by the framework to provide the input model.
        /// </summary>
        /// <value>
        /// The input model.
        /// </value>
        TInput Input { set; }
    }
}