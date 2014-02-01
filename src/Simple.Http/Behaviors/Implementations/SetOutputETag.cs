namespace Simple.Http.Behaviors.Implementations
{
    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetOutputETag
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static void Impl(IETag handler, IContext context)
        {
            if (!string.IsNullOrWhiteSpace(handler.OutputETag))
            {
                context.Response.SetETag(handler.OutputETag);
            }
        }
    }
}