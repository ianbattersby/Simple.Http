namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Protocol;

    class StubSetCache
    {
        public static bool Called;

        public static bool Impl(IContext c)
        {
            return Called = true;
        }
    }
}