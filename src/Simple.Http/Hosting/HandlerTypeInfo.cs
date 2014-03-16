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

    internal sealed class HandlerTypeInfo
    {
        private readonly Type type;

        public HandlerTypeInfo(Type type)
        {
            this.type = type;
        }

        public HandlerTypeInfo(Type type, int priority)
        {
            this.type = type;
            this.Priority = priority;
        }

        public int Priority { get; private set; }

        public Type HandlerType
        {
            get
            {
                return this.type;
            }
        }

        public HandlerTypeInfo SetPriority(int priority)
        {
            return new HandlerTypeInfo(this.type, priority);
        }
    }
}