namespace Simple.Http.Behaviors
{
    using Protocol;

    /// <summary>
    /// Indicates that a handler exposes caching information.
    /// </summary>
    [ResponseBehavior(typeof(Implementations.SetCacheOptions))]
    public interface ICacheability
    {
        /// <summary>
        /// Gets the cache options.
        /// </summary>
        CacheOptions CacheOptions { get; }
    }
}