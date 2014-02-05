// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryEx.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the DictionaryEx type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.OwinSupport
{
    using System.Collections.Generic;

    internal static class DictionaryEx
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}