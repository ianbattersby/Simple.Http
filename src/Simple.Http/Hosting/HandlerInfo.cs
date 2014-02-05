// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlerInfo.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Provides useful information about handlers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Hosting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides useful information about handlers.
    /// </summary>
    public sealed class HandlerInfo
    {
        private readonly Type handlerType;
        private readonly string httpMethod;
        private readonly IDictionary<string, string> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerInfo"/> class.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public HandlerInfo(Type handlerType, string httpMethod)
            : this(handlerType, new Dictionary<string, string>(), httpMethod)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerInfo"/> class.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public HandlerInfo(Type handlerType, IDictionary<string, string> variables, string httpMethod)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException("handlerType");
            }

            if (httpMethod == null)
            {
                throw new ArgumentNullException("httpMethod");
            }

            this.handlerType = handlerType;
            this.variables = variables ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.httpMethod = httpMethod;
        }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        public string HttpMethod
        {
            get { return this.httpMethod; }
        }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        public IDictionary<string, string> Variables
        {
            get { return this.variables; }
        }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        /// <value>
        /// The type of the handler.
        /// </value>
        public Type HandlerType
        {
            get { return this.handlerType; }
        }
    }
}