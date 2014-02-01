namespace Simple.Http.CodeGeneration
{
    using System;
    using System.Reflection;
    using Behaviors.Implementations;

    class MethodLookup : IMethodLookup
    {
        public MethodInfo SetInput { get { return Get(typeof (SetInput)); } }
        public MethodInfo SetInputETag { get { return Get(typeof (SetInputETag)); } } 
        public MethodInfo SetOutputETag { get { return Get(typeof (SetOutputETag)); } }
        public MethodInfo SetLastModified { get { return Get(typeof (SetLastModified)); } }
        public MethodInfo SetIfModifiedSince { get { return Get(typeof (SetIfModifiedSince)); } }
        public MethodInfo WriteStatusCode { get { return Get(typeof(WriteStatusCode)); } }
        public MethodInfo SetCache { get { return Get(typeof (SetCacheOptions)); } }
        public MethodInfo Redirect { get { return Get(typeof (Redirect)); } }
        public MethodInfo WriteStreamResponse { get { return Get(typeof (WriteStreamResponse)); } }
        public MethodInfo WriteRawHtml { get { return Get(typeof (WriteRawHtml)); } }
        public MethodInfo WriteOutput { get { return Get(typeof (WriteOutput)); } }
        public MethodInfo WriteView { get { return Get(typeof (WriteView)); } }

        private static MethodInfo Get(Type type)
        {
            return type.GetMethod("Impl", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        }
    }
}