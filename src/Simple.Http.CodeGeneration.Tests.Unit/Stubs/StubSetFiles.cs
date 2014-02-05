namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using Behaviors;
    using Protocol;

    class StubSetFiles
    {
        public static bool Called;
        public static bool Impl(IUploadFiles e, IContext c)
        {
            return Called = true;
        }
    }
}