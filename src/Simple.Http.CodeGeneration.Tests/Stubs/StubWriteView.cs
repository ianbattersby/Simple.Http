namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Protocol;

    class StubWriteView
    {
        public static bool Called;
        public static bool Impl(object e, IContext c)
        {
            return Called = true;
        }
    }
}