namespace Simple.Http.Tests.Unit.CodeGeneration.Stubs
{
    using System;

    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    class StubSetInput
    {
        public static bool Called;
        
        public static Type WithType;

        public static bool Impl<T>(IInput<T> e, IContext c)
        {
            WithType = typeof(T);
            return Called = true;
        }
    }
}