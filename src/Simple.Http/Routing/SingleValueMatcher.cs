// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleValueMatcher.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the SingleValueMatcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    internal class SingleValueMatcher : MatcherBase
    {
        private readonly string trimmed;

        public SingleValueMatcher(string pattern)
            : base(pattern, 1)
        {
            this.trimmed = pattern.Trim('{', '}');
        }

        protected override bool OnMatch(string part, MatchData matchData)
        {
            matchData.SetVariable(this.trimmed, part);
            return true;
        }
    }
}