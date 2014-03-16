namespace Simple.Http.TestMocks
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Simple.Http.Protocol;

    public class MockRequest : IRequest
    {
        public MockRequest()
        {
            this.QueryString = new Dictionary<string, string[]>();
            this.HttpMethod = "GET";
            this.Host = "localhost";
        }

        public Uri Url { get; set; }

        public IDictionary<string, string[]> QueryString { get; set; }

        public Stream InputStream { get; set; }

        public string HttpMethod { get; set; }

        public IDictionary<string, string[]> Headers { get; set; }

        public IEnumerable<IPostedFile> Files { get; private set; }

        public string Host { get; private set; }
    }
}