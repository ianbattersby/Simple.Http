namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Protocol;

    class StubWriteView
    {
        public static bool Called;

        public static bool Impl(object e, IContext c)
        {
            return Called = true;
        }
    }
}