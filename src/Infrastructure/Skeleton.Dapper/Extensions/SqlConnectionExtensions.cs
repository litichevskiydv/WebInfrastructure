using System.Dynamic;
using System.Linq;
using Skeleton.Dapper.Extensions.PropertyInfoProviders;

namespace Skeleton.Dapper.Extensions
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public static class SqlConnectionExtensions
    {
        public static void BulkInsert(this SqlConnection connection, string tableName, 
            IReadOnlyCollection<object> source, IPropertyInfoProvider provider,
            int? batchSize = null, int? timeout = null)
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
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