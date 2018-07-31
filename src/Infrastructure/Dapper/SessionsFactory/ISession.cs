namespace Skeleton.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Extensions;

    public interface ISession : IDisposable
    {
        IEnumerable<TSource> Query<TSource>(string sql, object param = null, bool buffered = true, TimeSpan? commandTimeout = null,
            CommandType? commandType = null);

        Task<IEnumerable<TSource>> QueryAsync<TSource>(string sql, object param = null, TimeSpan? commandTimeout = null,
            CommandType? commandType = null);

        int Execute(string sql, object param = null, TimeSpan? commandTimeout = null, CommandType? commandType = null);
        Task<int> ExecuteAsync(string sql, object param = null, TimeSpan? commandTimeout = null, CommandType? commandType = null);

        void BulkInsert<TSource>(string tableName, IReadOnlyCollection<TSource> source,
            Func<IColumnsMappingConfigurator<TSource>, IColumnsMappingConfigurator<TSource>> columnsMappingSetup = null,
            int? batchSize = null, TimeSpan? timeout = null) where TSource : class;

        void Commit();
    }
}