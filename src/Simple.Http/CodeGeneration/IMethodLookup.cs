// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodLookup.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides methods for the code generator
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System.Reflection;

    /// <summary>
    /// Provides methods for the code generator
    /// </summary>
    public interface IMethodLookup
    {
        /// <summary>
        /// Gets a method to write the status code to the response.
        /// </summary>
        MethodInfo WriteStatusCode { get; }

        /// <summary>
        /// Gets a method to render a view to the response.
        /// </summary>
        MethodInfo WriteView { get; }
    }
}