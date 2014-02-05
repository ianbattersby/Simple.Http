// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlerTypeInfo.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the HandlerTypeInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Hosting
{
    using System;
    using System.Collections.Generic;

    internal sealed class HandlerTypeInfo
    {
        private readonly Type type;
        private readonly HashSet<string> respondsToTypes;
        private readonly HashSet<string> respondsWithTypes;
        private readonly int priority;

        public HandlerTypeInfo(Type type)
            : this(type, null, null)
        {
        }

        public HandlerTypeInfo(Type type, IEnumerable<string> respondsToTypes, IEnumerable<string> respondsWithTypes)
        {
            this.type = type;

            if (respondsToTypes != null)
            {
                this.respondsToTypes = new HashSet<string>(respondsToTypes, StringComparer.OrdinalIgnoreCase);

                if (this.respondsToTypes.Count == 0)
                {
                    this.respondsToTypes = null;
                }
            }

            if (respondsWithTypes != null)
            {
                this.respondsWithTypes = new HashSet<string>(respondsWithTypes, StringComparer.OrdinalIgnoreCase);

                if (this.respondsWithTypes.Count == 0)
                {
                    this.respondsWithTypes = null;
                }
            }
        }

        private HandlerTypeInfo(
            Type type,
            HashSet<string> respondsToTypes,
            HashSet<string> respondsWithTypes,
            int priority)
        {
            this.type = type;
            this.respondsToTypes = respondsToTypes;
            this.respondsWithTypes = respondsWithTypes;
            this.priority = priority;
        }

        public int Priority
        {
            get { return this.priority; }
        }

        public int Property { get; set; }

        public Type HandlerType
        {
            get { return this.type; }
        }

        public bool RespondsToAll
        {
            get { return this.respondsToTypes == null; }
        }

        public bool RespondsWithAll
        {
            get { return this.respondsWithTypes == null; }
        }

        public bool RespondsTo(string contentType)
        {
            return this.respondsToTypes != null && this.respondsToTypes.Contains(contentType);
        }

        public bool RespondsWith(string acceptType)
        {
            return this.respondsWithTypes != null && this.respondsWithTypes.Contains(acceptType);
        }

        public HandlerTypeInfo SetPriority(int thisPriority)
        {
            return new HandlerTypeInfo(this.type, this.respondsToTypes, this.respondsWithTypes, thisPriority);
        }
    }
}