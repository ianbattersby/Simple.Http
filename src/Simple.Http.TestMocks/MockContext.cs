namespace Simple.Http.TestMocks
{
    using System.Collections.Generic;

    using Simple.Http.Protocol;

    public class MockContext : IContext
    {
        private readonly IDictionary<string, object> variables = new Dictionary<string, object>();

        public MockContext()
        {
            this.Request = new MockRequest();
            this.Response = new MockResponse();
        }

        public IRequest Request { get; set; }

        public IResponse Response { get; set; }

        public IDictionary<string, object> Variables
        {
            get { return this.variables; }
        }
    }
}