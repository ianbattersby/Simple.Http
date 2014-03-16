namespace Simple.Http.Tests.Unit.CodeGeneration.Handlers
{
    using System;
    using System.IO;

    using Simple.Http.Behaviors;

    class TestRedirectHandler : IGet, IMayRedirect, IOutputStream
    {
        private readonly Status status;

        public TestRedirectHandler(Status status)
        {
            this.status = status;
        }

        public Status Get()
        {
            return this.status;
        }

        public string Location
        {
            get { throw new NotImplementedException(); }
        }

        public Stream Output
        {
            get { throw new NotImplementedException(); }
        }

        public string ContentType
        {
            get { throw new NotImplementedException(); }
        }

        public string ContentDisposition
        {
            get { throw new NotImplementedException(); }
        }
    }
}