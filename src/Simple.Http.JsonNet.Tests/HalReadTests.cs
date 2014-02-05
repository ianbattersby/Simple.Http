﻿namespace Simple.Http.JsonNet.Tests
{
    using System.Text;
    using System.IO;
    using HalJson.Tests;
    using JsonNet;
    using Xunit;

    public class HalReadTests
    {
        [Fact]
        public void ReadsBasicObject()
        {
            const string Source = @"{""name"":""Arthur Dent"",""location"":""Guildford""}";
            Person actual;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Source)))
            {
                stream.Position = 0;
                var target = new HalJsonMediaTypeHandler();
                actual = (Person) target.Read(stream, typeof (Person));
            }

            Assert.Equal("Arthur Dent", actual.Name);
            Assert.Equal("Guildford", actual.Location);
        }

        [Fact]
        public void ReadsObjectWithLinks()
        {
            const string Source =
                @"{""_links"": {""self"":""/person/42""}, ""name"":""Arthur Dent"",""location"":""Guildford""}";
            Person actual;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Source)))
            {
                stream.Position = 0;
                var target = new HalJsonMediaTypeHandler();
                actual = (Person) target.Read(stream, typeof (Person));
            }

            Assert.Equal("Arthur Dent", actual.Name);
            Assert.Equal("Guildford", actual.Location);
        }
    }
}
