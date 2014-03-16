namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Protocol;

    class StubWriteStatusCode
    {
        public static bool Called;

        public static bool Impl(Status s, IContext c)
        {
            return Called = true;
        }
    }
}