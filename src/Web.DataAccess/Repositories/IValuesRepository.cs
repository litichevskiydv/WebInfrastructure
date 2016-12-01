namespace Web.DataAccess.Repositories
{
    public interface IValuesRepository<TValue>
    {
        TValue GetOrDefault(int key);

        void Set(int key, TValue value);
    }
}