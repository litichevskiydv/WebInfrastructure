namespace Skeleton.Dapper.Extensions
{
    public interface IPropertyInfoProvider
    {
        int FieldCount { get; }
        string GetName(int ordinal);
        object GetValue(int ordinal, object current);
    }
}
