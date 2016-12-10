namespace Web.DataAccess.Repositories
{
    public interface IValuesRepository<TValue>
    {
        /// <summary>
        /// Return value for key from storage
        /// </summary>
        /// <param name="key">Storage key</param>
        /// <returns>Value for key</returns>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">Value with <paramref name="key"/> 
        /// does not exist in the storage.</exception>
        TValue Get(int key);

        /// <summary>
        /// Set value for key to storage
        /// </summary>
        /// <param name="key">Storage key</param>
        /// <param name="value">Value for key</param>
        void Set(int key, TValue value);
    }
}