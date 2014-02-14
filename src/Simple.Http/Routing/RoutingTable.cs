// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingTable.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Handles routing for hosts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hosting;

    /// <summary>
    /// Handles routing for hosts.
    /// </summary>
    internal class RoutingTable
    {
        private readonly MatcherCollection matchers = new MatcherCollection();
        private readonly Dictionary<string, IMatcher> statics = new Dictionary<string, IMatcher>(StringComparer.OrdinalIgnoreCase);

        public void Add(string template, Type type)
        {
            this.Add(template, new HandlerTypeInfo(type));
        }

        public void Add(string template, HandlerTypeInfo type)
        {
            var addMatchers = this.matchers;

            var parts = template.Trim('/').Split(new[] { '/' });

            if (parts.Length == 0)
            {
                return;
            }

            IMatcher matcher;

            if (this.statics.ContainsKey(parts[0]))
            {
                matcher = this.statics[parts[0]];
            }
            else if (addMatchers.Contains(parts[0]))
            {
                matcher = addMatchers[parts[0]];
            }
            else
            {
                matcher = MatcherFactory.Create(parts[0]);

                if (matcher is StaticMatcher)
                {
                    this.statics.Add(parts[0], matcher);
                }
                else
                {
                    addMatchers.Add(matcher);
                }
            }

            if (parts.Length == 1)
            {
                matcher.AddTypeInfo(type);
                return;
            }

            matcher.Add(parts, 1).AddTypeInfo(type);
        }

        /// <summary>
        /// Gets the type of handler for the specified URL.
        /// </summary>
        /// <param name="url">The URL to match upon.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>The matched handler type.</returns>
        public Type GetHandlerTypeForUrl(string url, out IDictionary<string, string> variables)
        {
            variables = null;
            url = url.Trim('/');

            var nextIndex = url.IndexOf('/');
            var part = nextIndex >= 0 ? url.Substring(0, nextIndex) : url;

            IMatcher matcher;

            var matchData = new MatchData();
            var found = false;

            if (this.statics.TryGetValue(part, out matcher))
            {
                found = matcher.Match(part, url, nextIndex, matchData);
            }

            if (!found)
            {
                found = this.matchers.Aggregate(false, (current, t) => t.Match(part, url, nextIndex, matchData) || current);
            }

            if (!found)
            {
                return null;
            }

            variables = matchData.Variables;

            if (matchData.Single != null)
            {
                return matchData.Single.HandlerType;
            }

            return null;
        }

        public HashSet<Type> GetAllTypes()
        {
            var set = new HashSet<Type>();

            this.AddSub(set, this.statics.Values);
            this.AddSub(set, this.matchers);

            return set;
        }

        private void AddSub(HashSet<Type> set, IEnumerable<IMatcher> matchers)
        {
            foreach (var matcher in matchers)
            {
                if (matcher.Items != null)
                {
                    foreach (var typeInfo in matcher.Items)
                    {
                        set.Add(typeInfo.HandlerType);
                    }
                }

                this.AddSub(set, matcher.Matchers);

                var matcherBase = matcher as MatcherBase;

                if (matcherBase != null)
                {
                    this.AddSub(set, matcherBase.StaticMatchers);
                }
            }
        }
    }
}