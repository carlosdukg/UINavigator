namespace UINavigator.Services
{
    /// <summary>
    /// Local cache service
    /// </summary>
    public interface IMemCache
    {
        /// <summary>
        /// Get cached value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T? Get<T>(string key);
        
        /// <summary>
        /// Cache value locally in memory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);
    }
}
