// --------------------------------------------------------internal ------------------------------------------------------------
// <copyright file="MatcherFactory.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the MatcherFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;

    internal class MatcherFactory
    {
        public static IMatcher Create(string pattern)
        {
            if (!pattern.Contains("{"))
            {
                return new StaticMatcher(pattern);
            }

            if (pattern.StartsWith("{") && pattern.EndsWith("}") && pattern.IndexOf('{', 1) == -1)
            {
                return new SingleValueMatcher(pattern);
            }
            else
            {
                throw new NotSupportedException("Complex patterns not supported yet.");
            }
        }
    }
}