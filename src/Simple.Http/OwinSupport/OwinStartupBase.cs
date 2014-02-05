// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwinStartupBase.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the OwinStartupBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.OwinSupport
{
    using System;

    using Owin;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public abstract class OwinStartupBase
    {
        private readonly Action<IAppBuilder> builder;

        protected OwinStartupBase()
        {
            this.builder = builder => builder.Use(new Func<AppFunc, AppFunc>(ignoreNextApp => (AppFunc)Application.Run));
        }

        protected OwinStartupBase(Action<IAppBuilder> builder)
        {
            this.builder = builder;
        }

        protected Action<IAppBuilder> Builder
        {
            get
            {
                return this.builder;
            }
        }

        public void Configuration(IAppBuilder app)
        {
            this.builder.Invoke(app);
        }
    }
}