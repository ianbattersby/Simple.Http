namespace Simple.Http.CodeGeneration.Tests.Stubs
{
    using System;
    using Behaviors;
    using Protocol;

    class StubSetInput
    {
        public static bool Called;
        public static Type WithType;
        public static bool Impl<T>(IInput<T> e, IContext c)
        {
            WithType = typeof (T);
            return Called = true;
        }
    }
}