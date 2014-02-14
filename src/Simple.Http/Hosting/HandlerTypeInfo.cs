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

        public Type HandlerType
        {
            get
            {
                return this.type;
            }
        }
    }
}