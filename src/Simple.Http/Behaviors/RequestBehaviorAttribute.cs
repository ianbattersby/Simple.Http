﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestBehaviorAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Should be applied to behavior interfaces that work in the Request phase of a request/response cycle.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors
{
    using System;

    /// <summary>
    /// Should be applied to behavior interfaces that work in the Request phase of a request/response cycle.
    /// </summary>
    /// <remarks>The Request phase occurs before a handler has been run.</remarks>
    public sealed class RequestBehaviorAttribute : BehaviorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="implementingType">The type that implements the behavior for the interface.</param>
        public RequestBehaviorAttribute(Type implementingType)
            : base(implementingType)
        {
        }

        /// <summary>
        /// Gets the <see cref="RequestBehaviorAttribute"/> for the specified type.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <returns>The <see cref="RequestBehaviorAttribute"/>, or <c>null</c> if it is not applied to the type.</returns>
        public static RequestBehaviorAttribute Get(Type type)
        {
            return GetCustomAttribute(type, typeof(RequestBehaviorAttribute)) as RequestBehaviorAttribute;
        }
    }
}