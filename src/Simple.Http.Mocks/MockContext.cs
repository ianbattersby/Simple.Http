namespace Simple.Http.Mocks
{
    using System.Collections.Generic;

    using Simple.Http.Protocol;

    public class MockContext : IContext
    {
        private readonly IDictionary<string, object> variables = new Dictionary<string, object>();

        public MockContext()
        {
            Request = new MockRequest();
            Response = new MockResponse();
        }
        public IRequest Request { get; set; }

        public IResponse Response { get; set; }
        public IDictionary<string, object> Variables
        {
            get { return this.variables; }
        }
    }
}