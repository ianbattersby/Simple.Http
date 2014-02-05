// -----------------------------------------------------------------------------------------------internal ---------------------
// <copyright file="Comparer.cs" company="Mark Rendle and Ian Battersby.">
//   Copyright (C) Mark Rendle and Ian Battersby 2014 - All Rights Reserved.
// </copyright>
// <summary>
//   Defines the Comparer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Simple.Http.Helpers
{
    using System;
    using System.Collections.Generic;

    internal sealed class Comparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> compare;

        public Comparer(Func<T, T, int> compare)
        {
            if (compare == null)
            {
                throw new ArgumentNullException("compare");
            }

            this.compare = compare;
        }

        public int Compare(T x, T y)
        {
            return this.compare(x, y);
        }
    }
}