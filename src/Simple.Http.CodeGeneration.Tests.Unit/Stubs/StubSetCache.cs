namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Protocol;

    class StubSetCache
    {
        public static bool Called;
        public static bool Impl(IContext c)
        {
            return Called = true;
        }
    }
}