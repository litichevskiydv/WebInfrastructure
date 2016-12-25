namespace Web.DataAccess.Repositories.Impl
{
    using System.Collections.Concurrent;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class InMemoryValuesRepository<TValue> : IValuesRepository<TValue>
    {
        private readonly ConcurrentDictionary<int, TValue> _storage;

        public InMemoryValuesRepository()
        {
            _storage = new ConcurrentDictionary<int, TValue>();
        }

        public TValue Get(int key)
        {
            return _storage[key];
        }

        public void Set(int key, TValue value)
        {
            _storage.AddOrUpdate(key, value, (k, v) => value);
        }

        public void Delete(int key)
        {
            TValue value;
            _storage.TryRemove(key, out value);
        }
    }
}