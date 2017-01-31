namespace Web.DataAccess.Repositories
{
    using System.Collections.Generic;

    public interface IValuesRepository<TValue>
    {
        /// <summary>
        /// Returns all stored values
        /// </summary>
        /// <returns>All stored values</returns>
        IEnumerable<TValue> Get();

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

        /// <summary>
        /// Delete value by key from storage
        /// </summary>
        /// <param name="key">Storage key</param>
        void Delete(int key);
    }
}