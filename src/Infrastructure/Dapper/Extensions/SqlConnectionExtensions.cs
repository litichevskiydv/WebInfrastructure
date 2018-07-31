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
        private static SqlBulkCopy CreateSqlBulkCopy(
            SqlConnection connection,
            string tableName,
            SqlTransaction transaction,
            int? batchSize,
            TimeSpan? timeout)
        {
            var bulkCopy = transaction == null
                ? new SqlBulkCopy(connection)
                : new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);

            bulkCopy.DestinationTableName = tableName;
            bulkCopy.BatchSize = batchSize ?? 4096;
            bulkCopy.BulkCopyTimeout = timeout.HasValue ? (int) timeout.Value.TotalSeconds : 0;

            return bulkCopy;
        }

        public static void BulkInsert(
            this SqlConnection connection,
            string tableName,
            IReadOnlyCollection<object> source,
            Func<SqlBulkCopyColumnMappingCollection, IMappingInfoProvider> providerBuilder,
            SqlTransaction transaction = null,
            int? batchSize = null,
            TimeSpan? timeout = null)
        {
            if(connection == null)
                throw new ArgumentNullException(nameof(connection));
            if(string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            if (providerBuilder == null)
                throw new ArgumentNullException(nameof(providerBuilder));

            if (source.Any() == false)
                return;

            using (var bulkCopy = CreateSqlBulkCopy(connection, tableName, transaction, batchSize, timeout))
            {
                var provider = providerBuilder(bulkCopy.ColumnMappings);
                using (var reader = new CollectonReader(source, provider))
                    bulkCopy.WriteToServer(reader);
            }
        }

        public static void BulkInsert<TSource>(
            this SqlConnection connection,
            string tableName,
            IReadOnlyCollection<TSource> source,
            Func<IColumnsMappingConfigurator<TSource>, IColumnsMappingConfigurator<TSource>> columnsMappingSetup = null,
            SqlTransaction transaction = null,
            int? batchSize = null,
            TimeSpan? timeout = null)
            where TSource : class
        {
            if (columnsMappingSetup != null)
                BulkInsert(
                    connection, tableName, source,
                    x =>
                    {
                        var provider = new ManualConfiguredStrictTypeMappingInfoProvider<TSource>(x);
                        columnsMappingSetup(provider);
                        return provider;
                    },
                    transaction, batchSize, timeout
                );
            else if (typeof(TSource) == typeof(ExpandoObject))
                BulkInsert(
                    connection, tableName, source,
                    x => new ExpandoObjectMappingInfoProvider(source.First() as Dictionary<string, object>, x),
                    transaction, batchSize, timeout
                );
            else
                BulkInsert(
                    connection, tableName, source,
                    x => new StrictTypeMappingInfoProvider(source.First().GetType(), x),
                    transaction, batchSize, timeout
                );
        }
    }
}