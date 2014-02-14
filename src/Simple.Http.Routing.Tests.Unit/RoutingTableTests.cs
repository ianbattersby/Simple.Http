namespace Simple.Http.Routing.Tests.Unit
{
    using System.Collections.Generic;

    using Simple.Http.Routing;

    using Xunit;

    public class RoutingTableTests
    {
        [Fact]
        public void MatchesStaticUrl()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/", expected);
            IDictionary<string, string> matches;
            var actual = target.GetHandlerTypeForUrl("/", out matches);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MatchesStaticUrlNotEndingInSlash()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/test", expected);
            IDictionary<string, string> matches;
            var actual = target.GetHandlerTypeForUrl("/test", out matches);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MatchesStaticUrlEndingInSlash()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/test", expected);
            IDictionary<string, string> matches;
            var actual = target.GetHandlerTypeForUrl("/test/", out matches);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MatchesVanityUrlWithoutTrailingSlash()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/{Name}", expected);
            IDictionary<string, string> variables;
            var actual = target.GetHandlerTypeForUrl("/test", out variables);
            Assert.Equal(expected, actual);
            Assert.Equal("test", variables["Name"]);
        }

        [Fact]
        public void MatchesVanityUrlWithTrailingSlash()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/{Name}", expected);
            IDictionary<string, string> variables;
            var actual = target.GetHandlerTypeForUrl("/test/", out variables);
            Assert.Equal(expected, actual);
            Assert.Equal("test", variables["Name"]);
        }

        [Fact]
        public void MatchesDynamicUrlWithOneVariable()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/tests/{Id}", expected);
            IDictionary<string, string> variables;
            var actual = target.GetHandlerTypeForUrl("/tests/1", out variables);
            Assert.Equal(expected, actual);
            Assert.Equal("1", variables["Id"]);
        }

        [Fact]
        public void MatchesDynamicUrlWithTwoVariables()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/tests/{Year}/{Month}", expected);
            IDictionary<string, string> variables;
            var actual = target.GetHandlerTypeForUrl("/tests/2012/2", out variables);
            Assert.Equal(expected, actual);
            Assert.Equal("2012", variables["Year"]);
            Assert.Equal("2", variables["Month"]);
        }

        [Fact]
        public void MatchesDynamicUrlWithTrailingValues()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/tests/{Id}/bar", expected);
            IDictionary<string, string> variables;
            var actual = target.GetHandlerTypeForUrl("/tests/1/bar", out variables);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MatchesDynamicUrlWithTrailingValuesAheadOfMultiValue()
        {
            var target = new RoutingTable();
            var expected = typeof(RoutingTableTests);
            target.Add("/tests/{Year}/{Month}", typeof(int));
            target.Add("/tests/{Id}/bar", expected);
            IDictionary<string, string> variables;
            var actual = target.GetHandlerTypeForUrl("/tests/1/bar", out variables);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MatchesUrlWhenTwoRegexesHaveSameNumberOfGroups()
        {
            var target = new RoutingTable();
            var expectedFoo = typeof(int);
            var expectedBar = typeof(string);
            target.Add("/tests/{Id}/foo", expectedFoo);
            target.Add("/tests/{Id}/bar", expectedBar);
            IDictionary<string, string> variables;
            Assert.Equal(expectedFoo, target.GetHandlerTypeForUrl("/tests/1/foo", out variables));
            Assert.Equal(expectedBar, target.GetHandlerTypeForUrl("/tests/1/bar", out variables));
        }
    }
}