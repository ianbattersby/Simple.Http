// ---------------------------------------------------------------------------------------------------internal -----------------
// <copyright file="MatcherCollection.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the MatcherCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Routing
{
    using System;
    using System.Collections.ObjectModel;

    internal class MatcherCollection : KeyedCollection<string, IMatcher>
    {
        public MatcherCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        protected override string GetKeyForItem(IMatcher item)
        {
            return item.Pattern;
        }
    }
}