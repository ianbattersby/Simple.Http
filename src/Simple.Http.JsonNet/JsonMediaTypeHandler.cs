// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonMediaTypeHandler.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the JsonMediaTypeHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.JsonNet
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
    using Simple.Http.Links;
    using Simple.Http.MediaTypeHandling;

    [MediaTypes(MediaType.Json, "application/*+json")]
    public class JsonMediaTypeHandler : IMediaTypeHandler
    {
        private static readonly Lazy<HashSet<Type>> KnownTypes = new Lazy<HashSet<Type>>(GetKnownTypes);

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
                                                                                {
                                                                                    DateFormatHandling =
                                                                                        DateFormatHandling.IsoDateFormat,
                                                                                    ReferenceLoopHandling =
                                                                                        ReferenceLoopHandling.Ignore,
                                                                                    ContractResolver =
                                                                                        new CamelCasePropertyNamesContractResolver()
                                                                                };

        public static JsonSerializerSettings Settings
        {
            get { return SerializerSettings; }
        }

        private static HashSet<Type> GetKnownTypes()
        {
            var q = ExportedTypeHelper.FromCurrentAppDomain(LinkAttributeBase.Exists)
                              .SelectMany(LinkAttributeBase.Get)
                              .Select(l => l.ModelType);

            return new HashSet<Type>(q);
        }

        public object Read(Stream inputStream, Type inputType)
        {
            // pass the combined resolver strategy into the settings object
            using (var streamReader = new StreamReader(inputStream))
            {
                return JsonConvert.DeserializeObject(streamReader.ReadToEnd(), inputType, SerializerSettings);
            }
        }

        public Task Write(IContent content, Stream outputStream)
        {
            if (content.Model != null)
            {
                var linkConverters = LinkConverter.CreateForGraph(
                    content.Model.GetType(),
                    KnownTypes.Value,
                    LinkHelper.GetLinksForModel,
                    Settings.ContractResolver);

                var settings = new JsonSerializerSettings
                                   {
                                       Converters = linkConverters,
                                       ContractResolver = Settings.ContractResolver,
                                       DateFormatHandling = Settings.DateFormatHandling,
                                       ReferenceLoopHandling = Settings.ReferenceLoopHandling,
                                   };
                var json = JsonConvert.SerializeObject(content.Model, settings);
                var buffer = Encoding.UTF8.GetBytes(json);
                
                return outputStream.WriteAsync(buffer, 0, buffer.Length);
            }

            return TaskHelper.Completed();
        }
    }
}