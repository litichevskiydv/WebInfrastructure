namespace Skeleton.Dapper.Extensions
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public static class SqlConnectionExtensions
    {
        public static void BulkInsert<TSource>(this SqlConnection connection, string tableName, IReadOnlyCollection<TSource> source,
            int? batchSize = null, int? timeout = null) where TSource : class
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = batchSize ?? 4096;
                bulkCopy.BulkCopyTimeout = timeout ?? 0;

                using (var reader = new CollectionReader<TSource>(source))
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(i, reader.GetName(i)));
                    bulkCopy.WriteToServer(reader);
                }
            }
        }
    }
}