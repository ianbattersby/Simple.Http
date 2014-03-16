namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    class StubWriteRawHtml
    {
        public static bool Called;

        public static bool Impl(IOutput<RawHtml> e, IContext c)
        {
            return Called = true;
        }
    }
}