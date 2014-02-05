namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Behaviors;
    using Protocol;

    class StubSetUserCookie
    {
        public static bool Called;
        public static bool Impl(ILogin e, IContext c)
        {
            return Called = true;
        }
    }
}