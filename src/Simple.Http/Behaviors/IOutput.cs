// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOutput.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Adds output functionality to an handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors
{
    /// <summary>
    /// Adds output functionality to an handler.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    [OutputBehavior(typeof(Implementations.WriteOutput))]
    public interface IOutput<out TOutput>
    {
        /// <summary>
        /// Gets the output.
        /// </summary>
        TOutput Output { get; }
    }
}