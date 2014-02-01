namespace Simple.Http.Behaviors.Implementations
{
    using Behaviors;
    using Protocol;

    /// <summary>
    /// This type supports the framework directly and should not be used from your code.
    /// </summary>
    public static class SetLastModified
    {
        /// <summary>
        /// This method supports the framework directly and should not be used from your code
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static void Impl(IModified handler, IContext context)
        {
            if (handler.LastModified.HasValue)
            {
                context.Response.SetLastModified(handler.LastModified.Value);
            }
        }
    }
}