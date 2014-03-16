namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    class StubSetLastModified
    {
        public static bool Called;

        public static bool Impl(IETag e, IContext c)
        {
            return Called = true;
        }
    }
}