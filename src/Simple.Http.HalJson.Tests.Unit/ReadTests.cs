namespace Simple.Http.HalJson.Tests.Unit
{
    using System.IO;
    using System.Text;

    using Simple.Http.JsonNet;

    using Xunit;

    public class ReadTests
    {
        [Fact]
        public void ReadsBasicObject()
        {
            const string Source = @"{""name"":""Arthur Dent"",""location"":""Guildford""}";

            Person actual;
            
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(Source)))
            {
                stream.Position = 0;
                var target = new HalJsonMediaTypeHandler();
                actual = (Person)target.Read(stream, typeof(Person));
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
            
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(Source)))
            {
                stream.Position = 0;
                var target = new HalJsonMediaTypeHandler();
                actual = (Person)target.Read(stream, typeof(Person));
            }

            Assert.Equal("Arthur Dent", actual.Name);
            Assert.Equal("Guildford", actual.Location);
        }
    }
}
