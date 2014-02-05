// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRawHtml.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the WriteRawHtml type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System.Linq;
    using System.Text;

    using Simple.Http.Behaviors;
    using Simple.Http.Helpers;
    using Simple.Http.MediaTypeHandling;
    using Simple.Http.Protocol;

    internal static class WriteRawHtml
    {
        internal static void Impl(IOutput<RawHtml> handler, IContext context)
        {
            context.Response.SetContentType(
                context.Request.GetAccept().FirstOrDefault(
                    at => at == MediaType.Html || at == MediaType.XHtml) ?? "text/html");

            if (context.Request.HttpMethod.Equals("HEAD"))
            {
                return;
            }

            context.Response.WriteFunction = stream =>
                {
                    var bytes = Encoding.UTF8.GetBytes(handler.Output.ToString());
                    return stream.WriteAsync(bytes, 0, bytes.Length);
                };
        }
    }
}