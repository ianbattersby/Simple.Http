// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwinHelpers.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the OwinHelpers type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.OwinSupport
{
    using System;

    using Owin;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public static class OwinHelpers
    {
        public static void UseSimpleWeb(this IAppBuilder app)
        {
            app.Use(new Func<AppFunc, AppFunc>(ignoreNextApp => (AppFunc)Application.Run));
        }
    }
}