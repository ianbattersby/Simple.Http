﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectToDictionary.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Extension method for copying an object's properties to a dictionary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.MediaTypeHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension method for copying an object's properties to a dictionary.
    /// </summary>
    public static class ObjectToDictionary
    {
        private static readonly ConcurrentDictionary<Type, Func<object, IDictionary<string, object>>> Converters =
            new ConcurrentDictionary<Type, Func<object, IDictionary<string, object>>>();

        /// <summary>
        /// Copies all readable properties from an object to a dictionary.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A dictionary representation of the object's properties.</returns>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var converter = Converters.GetOrAdd(obj.GetType(), CreateConverter);

            return converter(obj);
        }

        private static Func<object, IDictionary<string, object>> CreateConverter(Type type)
        {
            var toDictionaryMethod = type.GetMethod("ToDictionary", new Type[0]);
            if (toDictionaryMethod != null)
            {
                return obj => toDictionaryMethod.Invoke(obj, null) as IDictionary<string, object>;
            }

            return obj =>
                       {
                           var properties =
                               obj.GetType().GetProperties().Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                                   .ToList();

                           var dictionary = new Dictionary<string, object>(properties.Count);

                           foreach (var property in properties)
                           {
                               dictionary.Add(property.Name, property.GetValue(obj, null));
                           }

                           return dictionary;
                       };
        }
    }
}