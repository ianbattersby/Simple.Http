// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkBuilder.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the ILinkBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Links
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Helpers;

    internal class LinkBuilder : ILinkBuilder
    {
        public static readonly ILinkBuilder Empty = new EmptyLinkBuilder();
        private readonly IList<Link> templates;

        public LinkBuilder(IEnumerable<Link> templates)
        {
            this.templates = templates.ToArray();
        }

        public ICollection<Link> LinksForModel(object model)
        {
            var actuals =
                this.templates.Select(l => new Link(l.GetHandlerType(), BuildUri(model, l.Href), l.Rel, l.Type, l.Title)).Where(l => l.Href != null).ToList();
            return new ReadOnlyCollection<Link>(actuals);
        }
        
        public Link CanonicalForModel(object model)
        {
            return
                this.templates.Where(t => t.Rel == "self").Select(
                    l => new Link(l.GetHandlerType(), BuildUri(model, l.Href), l.Rel, l.Type, l.Title)).FirstOrDefault(l => l.Href != null);
        }

        private static string BuildUri(object model, string uriTemplate)
        {
            var queryStart = uriTemplate.IndexOf("?", StringComparison.Ordinal);
            var uri = new StringBuilder(uriTemplate);
            var variables = new HashSet<string>(
                UriTemplateHelper.ExtractVariableNames(uriTemplate),
                StringComparer.OrdinalIgnoreCase);

            if (variables.Count > 0)
            {
                foreach (var variable in variables)
                {
                    var prop = model.GetType().GetProperty(variable);

                    if (prop == null)
                    {
                        return null;
                    }

                    var sub = "{" + variable + "}";
                    var v = prop.GetValue(model, null);

                    if (v == null)
                    {
                        return null;
                    }

                    var value = v.ToString();

                    if (queryStart >= 0)
                    {
                        if (uriTemplate.IndexOf(sub, StringComparison.OrdinalIgnoreCase) > queryStart)
                        {
                            value = Uri.EscapeDataString(value);
                        }
                    }

                    uri.Replace(sub, value);
                }
            }

            return uri.ToString();
        }

        private class EmptyLinkBuilder : ILinkBuilder
        {
            private static readonly Link[] EmptyArray = new Link[0];

            public ICollection<Link> LinksForModel(object model)
            {
                return EmptyArray;
            }

            public Link CanonicalForModel(object model)
            {
                return null;
            }
        }
    }
}