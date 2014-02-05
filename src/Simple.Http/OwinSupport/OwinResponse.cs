// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwinResponse.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the OwinResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.OwinSupport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Simple.Http.Protocol;

    internal class OwinResponse : IResponse
    {
        public Status Status { get; set; }

        public Func<Stream, Task> WriteFunction { get; set; }

        public IDictionary<string, string[]> Headers { get; set; }
    }
}