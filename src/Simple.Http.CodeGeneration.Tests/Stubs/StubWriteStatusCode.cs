namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Protocol;

    class StubWriteStatusCode
    {
        public static bool Called;
        public static bool Impl(Status s, IContext c)
        {
            return Called = true;
        }
    }
}