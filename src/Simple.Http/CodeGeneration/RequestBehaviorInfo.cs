// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestBehaviorInfo.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the RequestBehaviorInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Behaviors;

    internal class RequestBehaviorInfo : BehaviorInfo
    {
        private static List<RequestBehaviorInfo> cache;

        public RequestBehaviorInfo(Type behaviorType, Type implementingType, Priority priority)
            : base(behaviorType, implementingType, priority)
        {
        }

        public static IEnumerable<RequestBehaviorInfo> GetInPriorityOrder()
        {
            if (cache == null)
            {
                var list = FindRequestBehaviorTypes().OrderBy(t => t.Priority).ToList();
                Interlocked.CompareExchange(ref cache, list, null);
            }

            return cache;
        }

        private static IEnumerable<RequestBehaviorInfo> FindRequestBehaviorTypes()
        {
            return
                BehaviorInfo.FindBehaviorTypes<RequestBehaviorAttribute, RequestBehaviorInfo>(
                    (behaviorType, implementingType, priority) => new RequestBehaviorInfo(behaviorType, implementingType, priority));
        }
    }
}