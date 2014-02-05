// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HalJsonMediaTypeHandler.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the HalJsonMediaTypeHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.HalJson
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Simple.Http.Helpers;
    using Simple.Http.MediaTypeHandling;

    [MediaTypes("application/hal+json")]
    public class HalJsonMediaTypeHandler : IMediaTypeHandler
    {
        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
                                                                             {
                                                                                 ReferenceLoopHandling =
                                                                                     ReferenceLoopHandling.Ignore,
                                                                                 ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                 NullValueHandling = NullValueHandling.Ignore,
                                                                             };

        private static JsonSerializerSettings settings;

        public static JsonSerializerSettings Settings
        {
            get { return settings ?? DefaultSettings; }
            set { settings = value; }
        }

        public object Read(Stream inputStream, Type inputType)
        {
            var serializer = JsonSerializer.Create(Settings);
            var streamReader = new StreamReader(inputStream);
            var reader = new JsonTextReader(streamReader);

            return serializer.Deserialize(reader, inputType);
        }

        public Task Write(IContent content, Stream outputStream)
        {
            if (ReferenceEquals(null, content.Model))
            {
                return TaskHelper.Completed();
            }

            var output = ProcessContent(content);

            var serializer = JsonSerializer.Create(Settings);
            var stringWriter = new StringWriter();
            var writer = new JsonTextWriter(stringWriter);
            
            serializer.Serialize(writer, output);
            
            var buffer = Encoding.UTF8.GetBytes(stringWriter.ToString());
            
            return outputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        private static object ProcessContent(IContent content)
        {
            var links = content.Links.ToList();
            
            if (links.Count == 0)
            {
                return content.Model;
            }
            
            var dictionary = content.Model.ToDictionary();
            
            dictionary.Add("_links", CreateHalLinks(links));
            
            return dictionary;
        }

        private static IDictionary<string, object> CreateHalLinks(IEnumerable<Links.Link> links)
        {
            var halLinks = new Dictionary<string, object>();

            foreach (var link in links)
            {
                if (halLinks.ContainsKey(link.Rel))
                {
                    var linkList = halLinks[link.Rel] as IList<HalLink>;
                    if (linkList == null)
                    {
                        linkList = new List<HalLink> { (HalLink)halLinks[link.Rel] };
                        halLinks.Add(link.Rel, linkList);
                    }

                    linkList.Add(new HalLink(link.Href, link.Title));
                }
                else
                {
                    halLinks.Add(link.Rel, new HalLink(link.Href, link.Title));
                }
            }
            
            return halLinks;
        }

        private class HalLink
        {
            private readonly string href;
            private readonly string title;

            public HalLink(string href, string title)
            {
                if (href == null)
                {
                    throw new ArgumentNullException("href");
                }

                this.href = href;
                this.title = string.IsNullOrWhiteSpace(title) ? null : title;
            }

            public string Href
            {
                get { return this.href; }
            }

            public string Title
            {
                get { return this.title; }
            }
        }
    }
}