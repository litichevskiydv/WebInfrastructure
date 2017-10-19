namespace Skeleton.Dapper.Extensions
{
    using System.Dynamic;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using PropertyInfoProviders;

    public static class SqlConnectionExtensions
    {
        public static void BulkInsert(
            this SqlConnection connection,
            string tableName,
            IReadOnlyCollection<object> source,
            IPropertyInfoProvider provider,
            SqlTransaction transaction = null,
            int? batchSize = null,
            int? timeout = null)
        {
            BulkInsertInternal(connection, tableName, source, provider, transaction, batchSize, timeout);
        }

        public static void BulkInsert<TSource>(
            this SqlConnection connection,
            string tableName,
            IReadOnlyCollection<TSource> source,
            SqlTransaction transaction = null,
            int? batchSize = null,
            int? timeout = null)
            where TSource : class
        {
            if (source.Any() == false)
                return;
            if (typeof(TSource) == typeof(ExpandoObject))
                BulkInsertInternal(
                    connection,
                    tableName, source, new ExpandoObjectPropertyInfoProvider(source.First() as Dictionary<string, object>),
                    transaction, batchSize, timeout);
            else
                BulkInsertInternal(
                    connection,
                    tableName, source, new StrictTypePropertyInfoProvider(source.First().GetType()),
                    transaction, batchSize, timeout);
        }

        private static void BulkInsertInternal(
            SqlConnection connection, 
            string tableName,
            IReadOnlyCollection<object> source, 
            IPropertyInfoProvider provider,
            SqlTransaction transaction = null,
            int? batchSize = null, 
            int? timeout = null)
        {
            using (var bulkCopy = transaction == null
                ? new SqlBulkCopy(connection)
                : new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = batchSize ?? 4096;
                bulkCopy.BulkCopyTimeout = timeout ?? 0;

                using (var reader = new CollectonReader(source, provider))
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(i, reader.GetName(i)));
                    bulkCopy.WriteToServer(reader);
                }
            }
        }
    }
}