// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BehaviorAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Base class for <see cref="RequestBehaviorAttribute" />, <see cref="ResponseBehaviorAttribute" /> and <see cref="OutputBehaviorAttribute" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors
{
    using System;

    /// <summary>
    /// Base class for <see cref="RequestBehaviorAttribute"/>, <see cref="ResponseBehaviorAttribute"/> and <see cref="OutputBehaviorAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public abstract class BehaviorAttribute : Attribute
    {
        private readonly Type implementingType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorAttribute"/> class.
        /// </summary>
        /// <param name="implementingType">The type which implements the behavior.</param>
        protected BehaviorAttribute(Type implementingType)
        {
            this.implementingType = implementingType;
        }

        /// <summary>
        /// Gets the type that implements the behaviour.
        /// </summary>
        /// <value>
        /// The implementing type.
        /// </value>
        public Type ImplementingType
        {
            get { return this.implementingType; }
        }

        /// <summary>
        /// Gets or sets the priority, allowing control over the order in which behaviors are processed.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public Priority Priority { get; set; }
    }
}