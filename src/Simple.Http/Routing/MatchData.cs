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
        
        public IDictionary<string, string> Variables { get; private set; }

        public List<HandlerTypeInfo> List { get; private set; }

        public HandlerTypeInfo Single { get; private set; }

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
    }
}