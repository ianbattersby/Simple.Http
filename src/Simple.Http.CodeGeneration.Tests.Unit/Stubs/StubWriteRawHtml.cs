namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Behaviors;
    using Protocol;

    class StubWriteRawHtml
    {
        public static bool Called;
        public static bool Impl(IOutput<RawHtml> e, IContext c)
        {
            return Called = true;
        }
    }
}