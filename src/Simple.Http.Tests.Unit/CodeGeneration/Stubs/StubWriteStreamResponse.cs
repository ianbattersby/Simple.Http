namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    class StubWriteStreamResponse
    {
        public static bool Called;

        public static bool Impl(IOutputStream e, IContext c)
        {
            return Called = true;
        }
    }
}