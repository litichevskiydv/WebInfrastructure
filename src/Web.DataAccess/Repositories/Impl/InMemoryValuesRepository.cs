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

        public TValue GetOrDefault(int key)
        {
            TValue value;
            return _storage.TryGetValue(key, out value) ? value : default(TValue);
        }

        public void Set(int key, TValue value)
        {
            _storage.AddOrUpdate(key, value, (k, v) => value);
        }
    }
}