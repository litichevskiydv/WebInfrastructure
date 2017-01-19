namespace Skeleton.Dapper.Extensions
{
    using System;

    public interface IPropertyInfoProvider
    {
        int FieldCount { get; }
        string GetName(int ordinal);
        Type GetFieldType(int ordinal);
        int GetOrdinal(string name);
        object GetValue(int ordinal, object current);
    }
}
