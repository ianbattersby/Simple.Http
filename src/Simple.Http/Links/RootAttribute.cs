// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootAttribute.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the RootAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Links
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RootAttribute : LinkAttributeBase
    {
        public RootAttribute(string uriTemplate = null)
            : base(null, uriTemplate)
        {
            this.Rel = "self";
        }

        public string Rel { get; set; }

        internal override string GetRel()
        {
            return this.Rel;
        }
    }
}