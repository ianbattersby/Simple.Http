// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaTypesAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Specifies which content types an implementation of <see cref="IMediaTypeHandler" /> is used for.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.MediaTypeHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Specifies which content types an implementation of <see cref="IMediaTypeHandler"/> is used for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MediaTypesAttribute : Attribute
    {
        private readonly string[] contentTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypesAttribute"/> class.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        public MediaTypesAttribute(params string[] contentTypes)
        {
            this.contentTypes = contentTypes;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        public string[] ContentTypes
        {
            get { return this.contentTypes; }
        }

        /// <summary>
        /// Gets a collection of <see cref="MediaTypesAttribute"/> instances for a specified type.
        /// </summary>
        /// <param name="type">The handler type to inspect.</param>
        /// <returns>A list of <see cref="MediaTypesAttribute"/> attributes applied to the type.</returns>
        public static IEnumerable<MediaTypesAttribute> Get(Type type)
        {
            return GetCustomAttributes(type, typeof(MediaTypesAttribute)).Cast<MediaTypesAttribute>();
        }
    }
}