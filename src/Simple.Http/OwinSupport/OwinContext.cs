// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwinContext.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the OwinContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.OwinSupport
{
    using System.Collections.Generic;
    using System.IO;

    using Simple.Http.Protocol;

    internal class OwinContext : IContext
    {
        public OwinContext(IDictionary<string, object> env)
        {
            this.Variables = env;
            this.Request = new OwinRequest(env, (IDictionary<string, string[]>)env[OwinKeys.RequestHeaders], (Stream)env[OwinKeys.RequestBody]);
            this.Response = new OwinResponse();
        }

        public IRequest Request { get; private set; }

        public IResponse Response { get; private set; }

        public IDictionary<string, object> Variables { get; private set; }
    }
}