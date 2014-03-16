﻿namespace Simple.Http.Tests.Unit
{
    using System.Linq;

    using Xunit;

    public class UriTemplateAttributeTests
    {
        [Fact]
        public void BuildsCompundUriTemplate()
        {
            var actual = UriTemplateAttribute.GetAllTemplates(typeof(CompoundTest)).ToList();

            Assert.Equal(4, actual.Count);
            Assert.Contains("/foo/quux/", actual);
            Assert.Contains("/foo/wibble/", actual);
            Assert.Contains("/bar/quux/", actual);
            Assert.Contains("/bar/wibble/", actual);
        }

        [Fact]
        public void StillFindsSingleTemplate()
        {
            var actual = UriTemplateAttribute.GetAllTemplates(typeof(NoBaseTest)).Single();

            Assert.Equal("/rock", actual);
        }

        [Fact]
        public void OverrideExcludesBase()
        {
            var actual = UriTemplateAttribute.GetAllTemplates(typeof(OverrideBaseTest)).Single();

            Assert.Equal("/override", actual);
        }

        [Fact]
        public void SkipsMiddleBase()
        {
            var actual = UriTemplateAttribute.GetAllTemplates(typeof(Bottom)).Single();

            Assert.Equal("/top/bottom", actual);
        }

        [UriTemplate("/foo/")]
        [UriTemplate("/bar/")]
        public abstract class BaseTest
        {

        }

        [UriTemplate("/quux/")]
        [UriTemplate("/wibble/")]
        public class CompoundTest : BaseTest
        {

        }

        [UriTemplate("/rock")]
        public class NoBaseTest
        {

        }

        [UriTemplate("/override", false)]
        public class OverrideBaseTest
        {

        }

        [UriTemplate("/top")]
        public abstract class Top
        {
        }

        public abstract class Middle : Top
        {
        }

        [UriTemplate("/bottom")]
        public class Bottom : Middle
        {

        }
    }
}