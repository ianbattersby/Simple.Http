namespace Simple.Http.Tests.Unit.CodeGeneration.Handlers
{
    using System;

    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    abstract class TestHandlerBase : IInput<string>, ICacheability, IETag, IModified
    {
        public string Input { get; set; }

        public CacheOptions CacheOptions
        {
            get { return CacheOptions.DisableCaching; }
        }

        public string InputETag
        {
            set { throw new NotImplementedException(); }
        }

        public string OutputETag
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? IfModifiedSince
        {
            set { throw new NotImplementedException(); }
        }

        public DateTime? LastModified
        {
            get { throw new NotImplementedException(); }
        }
    }

    class TestHandler : TestHandlerBase, IGet
    {
        private readonly Status status;

        public TestHandler(Status status)
        {
            this.status = status;
        }

        public Status Get()
        {
            return this.status;
        }
    }
}