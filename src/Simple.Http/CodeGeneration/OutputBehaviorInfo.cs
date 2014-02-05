// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputBehaviorInfo.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the OutputBehaviorInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Behaviors;

    internal class OutputBehaviorInfo : BehaviorInfo
    {
        private static List<OutputBehaviorInfo> cache;

        public OutputBehaviorInfo(Type behaviorType, Type implementingType, Priority priority)
            : base(behaviorType, implementingType, priority)
        {
        }

        public static IEnumerable<OutputBehaviorInfo> GetInPriorityOrder()
        {
            if (cache == null)
            {
                var list = FindOutputBehaviorTypes().OrderBy(t => t.Priority).ToList();
                Interlocked.CompareExchange(ref cache, list, null);
            }

            return cache;
        }

        private static IEnumerable<OutputBehaviorInfo> FindOutputBehaviorTypes()
        {
            return
                BehaviorInfo.FindBehaviorTypes<OutputBehaviorAttribute, OutputBehaviorInfo>(
                    (t, i, p) => new OutputBehaviorInfo(t, i, p));
        }
    }
}