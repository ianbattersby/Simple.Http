// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpMethodAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Specifies which HTTP method (e.g. GET, POST, HEAD) a Handler interface deals with.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Protocol
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Specifies which HTTP method (e.g. GET, POST, HEAD) a Handler interface deals with.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class HttpMethodAttribute : Attribute
    {
        private readonly string httpMethod;
        private readonly string method;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMethodAttribute"/> class.
        /// </summary>
        /// <param name="httpMethod">The HTTP Method.</param>
        public HttpMethodAttribute(string httpMethod)
            : this(httpMethod, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMethodAttribute"/> class.
        /// </summary>
        /// <param name="httpMethod">The HTTP Method.</param>
        /// <param name="method">The name of the entry-point method in the type.</param>
        public HttpMethodAttribute(string httpMethod, string method)
        {
            this.httpMethod = httpMethod;
            this.method = method ?? char.ToUpperInvariant(httpMethod[0]) + httpMethod.Substring(1).ToLowerInvariant();
        }

        /// <summary>
        /// Gets the entry-point method name.
        /// </summary>
        public string Method
        {
            get { return this.method; }
        }

        /// <summary>
        /// Gets the HTTP Method.
        /// </summary>
        public string HttpMethod
        {
            get { return this.httpMethod; }
        }

        /// <summary>
        /// Gets the <see cref="HttpMethodAttribute"/> of the specified type.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="httpMethod">The HTTP method to look for.</param>
        /// <param name="excludeInterfaces">If <c>true</c>, interfaces will not be included in the search.</param>
        /// <returns><c>null</c> if the attribute does not exist.</returns>
        public static HttpMethodAttribute GetHttpMethodAttr(Type type, string httpMethod, bool excludeInterfaces = false)
        {
            var customAttribute = GetCustomAttributes(type, typeof(HttpMethodAttribute), true)
                .Cast<HttpMethodAttribute>()
                .FirstOrDefault(a => a.HttpMethod.Equals(httpMethod, StringComparison.OrdinalIgnoreCase));

            return customAttribute ?? type.GetInterfaces().Select(i => GetHttpMethodAttr(i, httpMethod)).FirstOrDefault(a => a != null);
        }

        public static Type GetAttributedType(Type type, string httpMethod)
        {
            if (!TypeHasAttribute(type, httpMethod))
            {
                return type.GetInterfaces().FirstOrDefault(i => TypeHasAttribute(i, httpMethod));
            }

            return type;
        }

        private static bool TypeHasAttribute(Type type, string httpMethod)
        {
            return GetCustomAttributes(type, typeof(HttpMethodAttribute))
                .Cast<HttpMethodAttribute>()
                .Any(a => a.HttpMethod.Equals(httpMethod, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the entry-point method name for a handler type.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="httpMethod">The HTTP method to match against the type.</param>
        /// <returns>The value of the <see cref="Method"/> property, or <c>null</c> if the attribute is not applied to the type.</returns>
        public static MethodInfo GetMethod(Type type, string httpMethod)
        {
            var attr = GetHttpMethodAttr(type, httpMethod);
            return attr == null ? null : type.GetMethod(attr.Method);
        }

        /// <summary>
        /// Determines whether the <see cref="HttpMethodAttribute"/> is applied to the specified type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>true</c> if <see cref="HttpMethodAttribute"/> is applied to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAppliedTo(Type type)
        {
            return Attribute.IsDefined(type, typeof(HttpMethodAttribute), true) ||
                              type.GetInterfaces().Any(i => Attribute.IsDefined(i, typeof(HttpMethodAttribute), false));
        }

        /// <summary>
        /// Checks the <see cref="HttpMethodAttribute"/> attribute aplied to the handler type matches that specified.
        /// </summary>
        /// <param name="type">The handler type to inspect.</param>
        /// <param name="httpMethod">The <see cref="HttpMethodAttribute"/> to match.</param>
        /// <returns>True is a match exists.</returns>
        public static bool Matches(Type type, string httpMethod)
        {
            if (httpMethod != null)
            {
                var attribute = GetHttpMethodAttr(type, httpMethod);

                if (attribute != null)
                {
                    return attribute.HttpMethod.Equals(httpMethod, StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }
    }
}