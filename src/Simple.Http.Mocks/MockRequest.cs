namespace Simple.Http.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using Protocol;

    public class MockRequest : IRequest
    {
        public MockRequest()
        {
            QueryString = new Dictionary<string, string[]>();
			HttpMethod = "GET";
            Host = "localhost";
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