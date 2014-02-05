// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetInput.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   This type supports the framework directly and should not be used from your code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Behaviors.Implementations
{
    using Simple.Http.Behaviors;
    using Simple.Http.MediaTypeHandling;
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetInput
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <typeparam name="T">The input model type.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        public static void Impl<T>(IInput<T> handler, IContext context)
        {
            if (context.Request.InputStream.CanSeek && context.Request.InputStream.Length == 0)
            {
                return;
            }

            var mediaTypeHandlerTable = new MediaTypeHandlerTable();
            var mediaTypeHandler = mediaTypeHandlerTable.GetMediaTypeHandler(context.Request.GetContentType());

            handler.Input = (T)mediaTypeHandler.Read(context.Request.InputStream, typeof(T));
        }
    }
}