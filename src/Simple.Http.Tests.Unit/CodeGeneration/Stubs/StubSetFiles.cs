namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    class StubSetFiles
    {
        public static bool Called;

        public static bool Impl(IUploadFiles e, IContext c)
        {
            return Called = true;
        }
    }
}