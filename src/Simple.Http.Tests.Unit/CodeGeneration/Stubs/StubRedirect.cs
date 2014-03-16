namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    class StubRedirect
    {
        public static bool Called;

        public static bool Impl(IMayRedirect e, Status status, IContext c)
        {
            Called = true;

            return ((status.Code >= 301 && status.Code <= 303) || status.Code == 307);
        }
    }
}