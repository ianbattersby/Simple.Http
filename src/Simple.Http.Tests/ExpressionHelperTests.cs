﻿using System;

namespace Simple.Http.Tests
{
    using System.Linq.Expressions;
    using Helpers;
    using Xunit;

    public class ExpressionHelperTests
    {
        [Fact]
        public void GetsConstantValue()
        {
            Expression<Func<int>> target = () => 42;
            object actual;
            Assert.True(ExpressionHelper.TryGetValue(target.Body, out actual));
            Assert.Equal(42, actual);
        }

        [Fact]
        public void GetsPropertyValue()
        {
            const string Str = "Marvin";
            Expression<Func<int>> target = () => Str.Length;
            object actual;
            Assert.True(ExpressionHelper.TryGetValue(target.Body, out actual));
            Assert.Equal(6, actual);
        }
    }
}
