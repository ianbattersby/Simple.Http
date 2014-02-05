// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHelper.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Used for writing error messages to a response.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using Protocol;

    /// <summary>
    /// Used for writing error messages to a response.
    /// </summary>
    public class ErrorHelper
    {
        private readonly IContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHelper"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ErrorHelper(IContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Writes the error to the Response.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void WriteError(Exception exception)
        {
            Trace.TraceError(exception.Message);

            var httpException = exception as HttpException;

            if (httpException == null)
            {
                this.context.Response.Status = "500 Internal server error.";
            }
            else
            {
                this.context.Response.Status = string.Format("{0} {1}", httpException.ErrorCode, httpException.Message);
            }

            this.context.Response.SetContentType("text/html");
            this.context.Response.Write(exception.ToString());
        }
    }
}