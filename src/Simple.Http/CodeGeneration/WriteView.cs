// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteView.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the WriteView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;

    using Simple.Http.MediaTypeHandling;
    using Simple.Http.Protocol;

    internal static class WriteView
    {
        public static void Impl(object handler, IContext context)
        {
            WriteUsingMediaTypeHandler(handler, context);
        }

        private static void WriteUsingMediaTypeHandler(object handler, IContext context)
        {
            if (context.Request.HttpMethod == null)
            {
                throw new Exception("No HTTP Method given");
            }

            if (context.Request.HttpMethod.Equals("HEAD"))
            {
                return;
            }

            IMediaTypeHandler mediaTypeHandler;
            var acceptedTypes = context.Request.GetAccept();

            if (TryGetMediaTypeHandler(context, acceptedTypes, out mediaTypeHandler))
            {
                context.Response.SetContentType(mediaTypeHandler.GetContentType(acceptedTypes));

                context.Response.WriteFunction = stream =>
                    {
                        var content = new Content(context.Request.Url, handler, null);
                        return mediaTypeHandler.Write(content, stream);
                    };
            }
        }

        private static bool TryGetMediaTypeHandler(IContext context, IList<string> acceptedTypes, out IMediaTypeHandler mediaTypeHandler)
        {
            if (acceptedTypes == null || (acceptedTypes.Count == 1 && acceptedTypes[0].StartsWith("*/*")))
            {
                mediaTypeHandler = null;
                return false;
            }
            
            try
            {
                string matchedType;
                mediaTypeHandler = new MediaTypeHandlerTable().GetMediaTypeHandler(acceptedTypes, out matchedType);
            }
            catch (UnsupportedMediaTypeException)
            {
                context.Response.Status = "415 Unsupported media type requested.";
                mediaTypeHandler = null;
                return false;
            }
            
            return true;
        }
    }
}