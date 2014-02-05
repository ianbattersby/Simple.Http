namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Behaviors;
    using Protocol;
    using Xunit;

    class StubCheckAuthentication
    {
        public static bool Called;
        public static bool Impl(IRequireAuthentication e, IContext c)
        {
            Assert.NotNull(e);
            Assert.NotNull(c);
            return Called = true;
        }
    }
}