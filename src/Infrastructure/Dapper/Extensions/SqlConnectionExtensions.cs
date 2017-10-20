namespace Skeleton.Dapper.Extensions
{
    using System;
    using System.Dynamic;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using PropertyInfoProviders;

    public static class SqlConnectionExtensions
    {
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
                BulkInsert(
                    connection,
                    tableName, source, new ExpandoObjectPropertyInfoProvider(source.First() as Dictionary<string, object>),
                    transaction, batchSize, timeout);
            else
                BulkInsert(
                    connection,
                    tableName, source, new StrictTypePropertyInfoProvider(source.First().GetType()),
                    transaction, batchSize, timeout);
        }

        private static SqlBulkCopy CreateSqlBulkCopy(
            SqlConnection connection,
            string tableName,
            SqlTransaction transaction,
            int? batchSize,
            int? timeout)
        {
            var bulkCopy = transaction == null
                ? new SqlBulkCopy(connection)
                : new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);

            bulkCopy.DestinationTableName = tableName;
            bulkCopy.BatchSize = batchSize ?? 4096;
            bulkCopy.BulkCopyTimeout = timeout ?? 0;

            return bulkCopy;
        }

        public static void BulkInsert(
            this SqlConnection connection,
            string tableName,
            IReadOnlyCollection<object> source,
            IPropertyInfoProvider provider,
            SqlTransaction transaction = null,
            int? batchSize = null,
            int? timeout = null)
        {
            using (var bulkCopy = CreateSqlBulkCopy(connection, tableName, transaction, batchSize, timeout))
            using (var reader = new CollectonReader(source, provider))
            {
                for (var i = 0; i < reader.FieldCount; i++)
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(i, reader.GetName(i)));
                bulkCopy.WriteToServer(reader);
            }
        }

        public static void BulkInsert<TSource>(
            this SqlConnection connection,
            string tableName,
            IReadOnlyCollection<TSource> source,
            Func<IColumnsMappingConfigurator<TSource>, IColumnsMappingConfigurator<TSource>> columnsMappingSetup,
            SqlTransaction transaction = null,
            int? batchSize = null,
            int? timeout = null)
            where TSource : class
        {
            using (var bulkCopy = CreateSqlBulkCopy(connection, tableName, transaction, batchSize, timeout))
            {
                var provider = new ManualConfiguredStrictTypePropertyInfoProvider<TSource>(bulkCopy.ColumnMappings);
                columnsMappingSetup(provider);

                using (var reader = new CollectonReader(source, provider))
                    bulkCopy.WriteToServer(reader);
            }
        }
    }
}