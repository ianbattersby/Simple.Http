namespace Simple.Http.Tests.Unit
{
    using Simple.Http.Behaviors;

    using Xunit;

    public class GetHandlerTests
    {
        [Fact]
        public void GetRootWithHtmlReturnsHtml()
        {
        }
    }

    [UriTemplate("/")]
    public class RootHandler : IGet, IOutput<RawHtml>
    {
        public Status Get()
        {
            return Status.OK;
        }

        public RawHtml Output { get { return "<h1>Hello</h1>"; } }
    }
}
