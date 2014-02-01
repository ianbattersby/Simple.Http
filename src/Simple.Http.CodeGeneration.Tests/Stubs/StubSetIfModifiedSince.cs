namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Behaviors;
    using Protocol;

    class StubSetIfModifiedSince
    {
        public static bool Called;
        public static bool Impl(IETag e, IContext c)
        {
            return Called = true;
        }
    }
}