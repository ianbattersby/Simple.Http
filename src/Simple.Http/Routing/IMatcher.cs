// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMatcher.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the IMatcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System.Collections.Generic;

    using Simple.Http.Hosting;

    internal interface IMatcher
    {
        IList<HandlerTypeInfo> Items { get; }

        string Pattern { get; }

        MatcherCollection Matchers { get; }

        void AddTypeInfo(HandlerTypeInfo info);

        bool Match(string part, string template, int index, MatchData matchData);

        IMatcher Add(string[] parts, int index, int priority);
    }
}