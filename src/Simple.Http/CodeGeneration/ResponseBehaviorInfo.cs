// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseBehaviorInfo.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the ResponseBehaviorInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Behaviors;

    internal class ResponseBehaviorInfo : BehaviorInfo
    {
        private static List<ResponseBehaviorInfo> cache;

        public ResponseBehaviorInfo(Type behaviorType, Type implementingType, Priority priority)
            : base(behaviorType, implementingType, priority)
        {
        }

        public static IEnumerable<ResponseBehaviorInfo> GetInPriorityOrder(params ResponseBehaviorInfo[] defaults)
        {
            if (cache == null)
            {
                var list = FindResponseBehaviorTypes().Concat(defaults).OrderBy(r => r.Priority).ToList();
                Interlocked.CompareExchange(ref cache, list, null);
            }

            return cache;
        }

        private static IEnumerable<ResponseBehaviorInfo> FindResponseBehaviorTypes()
        {
            return
                BehaviorInfo.FindBehaviorTypes<ResponseBehaviorAttribute, ResponseBehaviorInfo>(
                    (t, i, p) => new ResponseBehaviorInfo(t, i, p));
        }
    }
}