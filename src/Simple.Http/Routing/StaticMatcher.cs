// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticMatcher.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the StaticMatcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;

    internal class StaticMatcher : MatcherBase
    {
        public StaticMatcher(string pattern)
            : base(pattern, 0)
        {
        }

        protected override bool OnMatch(string value, MatchData matchData)
        {
            return value.Equals(this.Pattern, StringComparison.OrdinalIgnoreCase);
        }
    }
}