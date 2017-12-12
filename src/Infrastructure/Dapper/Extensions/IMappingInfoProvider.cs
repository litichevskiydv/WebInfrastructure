namespace Skeleton.Dapper.Extensions
{
    using System.Data.SqlClient;

    public interface IMappingInfoProvider
    {
        object GetValue(int ordinal, object current);
        SqlBulkCopyColumnMappingCollection MappingsCollection { get; }
    }
}
