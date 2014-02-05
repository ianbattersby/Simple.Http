namespace Simple.Http.Tests
{
    using Behaviors;

    using Simple.Http.MediaTypeHandling;

    using Xunit;

    public class GetHandlerTests
    {
        [Fact]
        public void GetRootWithHtmlReturnsHtml()
        {
        }
    }



    [UriTemplate("/")]
    [RespondsWith(MediaType.Html)]
    public class RootHandler : IGet, IOutput<RawHtml>
    {
        public Status Get()
        {
            return Status.OK;
        }

        public RawHtml Output { get { return "<h1>Hello</h1>"; } }
    }
}
