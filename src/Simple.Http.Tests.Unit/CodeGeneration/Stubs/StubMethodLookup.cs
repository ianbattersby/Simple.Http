namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using System.Reflection;

    using Simple.Http.CodeGeneration;

    class StubMethodLookup : IMethodLookup
    {
        public MethodInfo SetInput { get { return typeof(StubSetInput).GetMethod("Impl"); } }

        public MethodInfo SetInputETag
        {
            get { return typeof(StubSetInputETag).GetMethod("Impl"); }
        }

        public MethodInfo SetOutputETag
        {
            get { return typeof(StubSetOutputETag).GetMethod("Impl"); }
        }

        public MethodInfo SetLastModified
        {
            get { return typeof(StubSetLastModified).GetMethod("Impl"); }
        }

        public MethodInfo SetIfModifiedSince
        {
            get { return typeof(StubSetIfModifiedSince).GetMethod("Impl"); }
        }

        public MethodInfo WriteStatusCode { get { return typeof(StubWriteStatusCode).GetMethod("Impl"); } }

        public MethodInfo SetFiles { get { return typeof(StubSetFiles).GetMethod("Impl"); } }

        public MethodInfo SetCache { get { return typeof(StubSetCache).GetMethod("Impl"); } }

        public MethodInfo Redirect { get { return typeof(StubRedirect).GetMethod("Impl"); } }

        public MethodInfo WriteStreamResponse { get { return typeof(StubWriteStreamResponse).GetMethod("Impl"); } }

        public MethodInfo WriteRawHtml { get { return typeof(StubWriteRawHtml).GetMethod("Impl"); } }

        public MethodInfo WriteView { get { return typeof(StubWriteView).GetMethod("Impl"); } }

        public MethodInfo WriteOutput { get { return typeof(StubWriteOutput).GetMethod("Impl"); } }
    }
}