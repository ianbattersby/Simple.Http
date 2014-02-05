﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetInput.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using Simple.Http.MediaTypeHandling;
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class GetInput
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <typeparam name="T">The input model type.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>The model de-serialized from the input stream.</returns>
        public static T Impl<T>(IContext context)
        {
            if (context.Request.InputStream.CanSeek && context.Request.InputStream.Length == 0)
            {
                return default(T);
            }

            var mediaTypeHandlerTable = new MediaTypeHandlerTable();
            var mediaTypeHandler = mediaTypeHandlerTable.GetMediaTypeHandler(context.Request.GetContentType());

            return (T)mediaTypeHandler.Read(context.Request.InputStream, typeof(T));
        }
    }
}