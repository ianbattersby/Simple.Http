// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatcherBase.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the MatcherBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;
    using System.Collections.Generic;

    using Simple.Http.Hosting;

    internal abstract class MatcherBase : IMatcher
    {
        private readonly MatcherCollection matchers = new MatcherCollection();
        private readonly Dictionary<string, IMatcher> statics = new Dictionary<string, IMatcher>(StringComparer.OrdinalIgnoreCase);
        private List<HandlerTypeInfo> typeInfos;

        protected MatcherBase(string pattern)
        {
            this.Pattern = pattern;
        }

        public IList<HandlerTypeInfo> Items
        {
            get
            {
                return this.typeInfos;
            }
        }

        public string Pattern { get; private set; }

        public MatcherCollection Matchers
        {
            get
            {
                return this.matchers;
            }
        }

        internal IEnumerable<IMatcher> StaticMatchers
        {
            get
            {
                return this.statics.Values;
            }
        }

        public void AddTypeInfo(HandlerTypeInfo typeInfo)
        {
            if (this.typeInfos == null)
            {
                this.typeInfos = new List<HandlerTypeInfo>();
            }

            this.typeInfos.Add(typeInfo);
        }

        public bool Match(string part, string value, int index, MatchData matchData)
        {
            if (!this.OnMatch(part, matchData))
            {
                return false;
            }

            if (index == -1)
            {
                if (this.typeInfos == null)
                {
                    return false;
                }

                matchData.Add(this.typeInfos);

                return true;
            }

            var nextIndex = value.IndexOf('/', ++index);
            part = nextIndex == -1 ? value.Substring(index) : value.Substring(index, nextIndex - index);

            IMatcher matcher;

            if (this.statics.TryGetValue(part, out matcher))
            {
                return matcher.Match(part, value, nextIndex, matchData);
            }

            var found = false;

            foreach (var t in this.matchers)
            {
                found = t.Match(part, value, nextIndex, matchData) || found;
            }

            return found;
        }

        protected abstract bool OnMatch(string part, MatchData matchData);

        public IMatcher Add(string[] parts, int index)
        {
            if (index >= parts.Length)
            {
                return this;
            }

            IMatcher matcher;

            if (!this.statics.TryGetValue(parts[index], out matcher))
            {
                if (this.matchers.Contains(parts[index]))
                {
                    matcher = this.matchers[parts[index]];
                }
                else
                {
                    matcher = MatcherFactory.Create(parts[index]);
                    if (matcher is StaticMatcher)
                    {
                        this.statics.Add(parts[index], matcher);
                    }
                    else
                    {
                        this.matchers.Add(matcher);
                    }
                }
            }

            return matcher.Add(parts, index + 1);
        }
    }
}