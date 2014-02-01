namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Behaviors;
    using Protocol;

    class StubWriteStreamResponse
    {
        public static bool Called;
        public static bool Impl(IOutputStream e, IContext c)
        {
            return Called = true;
        }
    }
}