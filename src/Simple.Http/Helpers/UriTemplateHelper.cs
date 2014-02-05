// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplateHelper.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the UriTemplateHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal static class UriTemplateHelper
    {
        public static IEnumerable<string> ExtractVariableNames(string template)
        {
            return new Regex("{([^}]*)}").Matches(template).Cast<Match>().Select(m => m.Value.Trim('{', '}'));
        }
    }
}