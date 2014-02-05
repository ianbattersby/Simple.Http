// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchData.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the MatchData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Simple.Http.Hosting;

    internal class MatchData
    {
        private bool set;

        private HandlerTypeInfo[] prioritised;

        public IDictionary<string, string> Variables { get; private set; }

        public List<HandlerTypeInfo> List { get; private set; }

        public HandlerTypeInfo Single { get; private set; }

        private IEnumerable<HandlerTypeInfo> PrioritiseList()
        {
            return this.prioritised ?? (this.prioritised = this.List.OrderBy(hti => hti.Priority).ToArray());
        }

        public void Add(IList<HandlerTypeInfo> typeInfos)
        {
            if (!this.set)
            {
                if (typeInfos.Count == 1)
                {
                    this.Single = typeInfos[0];
                }
                else
                {
                    this.List = new List<HandlerTypeInfo>(typeInfos);
                }

                this.set = true;
            }
            else
            {
                if (this.Single != null)
                {
                    this.List = typeInfos as List<HandlerTypeInfo> ?? typeInfos.ToList();
                    this.List.Insert(0, this.Single);
                    this.Single = null;
                }
                else
                {
                    this.List.AddRange(typeInfos);
                }
            }
        }

        public void SetVariable(string key, string value)
        {
            if (this.Variables == null)
            {
                this.Variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            if (this.Variables.ContainsKey(key))
            {
                // Append this value with a delimiter
                this.Variables[key] += "\t" + value;
            }
            else
            {
                this.Variables.Add(key, value);
            }
        }

        public Type ResolveByMediaTypes(string contentType, IList<string> acceptTypes)
        {
            if (contentType == null)
            {
                if (acceptTypes == null)
                {
                    var match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsToAll && hti.RespondsWithAll);

                    if (match != null)
                    {
                        return match.HandlerType;
                    }

                    return this.PrioritiseList().First().HandlerType;
                }

                return this.ResolveByAcceptTypes(acceptTypes);
            }

            if (acceptTypes == null)
            {
                return this.ResolveByContentType(contentType);
            }

            return this.ResolveByBoth(contentType, acceptTypes);
        }

        private Type ResolveByBoth(string contentType, IList<string> acceptTypes)
        {
            HandlerTypeInfo match;
            foreach (var acceptType in acceptTypes)
            {
                match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsTo(contentType) && hti.RespondsWith(acceptType));

                if (match != null)
                {
                    return match.HandlerType;
                }
            }

            foreach (var acceptType in acceptTypes)
            {
                match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsToAll && hti.RespondsWith(acceptType));

                if (match != null)
                {
                    return match.HandlerType;
                }
            }

            match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsWithAll && hti.RespondsTo(contentType))
                    ?? this.PrioritiseList().FirstOrDefault(hti => hti.RespondsToAll && hti.RespondsWithAll);

            return match == null ? null : match.HandlerType;
        }

        private Type ResolveByContentType(string contentType)
        {
            var match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsTo(contentType))
                        ?? this.PrioritiseList().FirstOrDefault(hti => hti.RespondsToAll);

            return match == null ? null : match.HandlerType;
        }

        private Type ResolveByAcceptTypes(IEnumerable<string> acceptTypes)
        {
            HandlerTypeInfo match;

            foreach (var acceptType in acceptTypes)
            {
                match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsWith(acceptType));

                if (match != null)
                {
                    return match.HandlerType;
                }
            }

            match = this.PrioritiseList().FirstOrDefault(hti => hti.RespondsWithAll);

            return match == null ? null : match.HandlerType;
        }
    }
}