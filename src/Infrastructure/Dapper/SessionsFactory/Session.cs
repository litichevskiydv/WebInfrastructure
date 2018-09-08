namespace Skeleton.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Extensions;
    using global::Dapper;

    public class Session : ISession
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        protected bool Disposed;

        public Session(IDbConnection connection, IDbTransaction transaction)
        {
            if(connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            _connection = connection;
            _transaction = transaction;
        }

        private void ValidateStatus(string objectName)
        {
            if (Disposed)
                throw new ObjectDisposedException(objectName);
        }

        private static int? ComputeCommandTimeoutInSeconds(TimeSpan? commandTimeout)
        {
            return commandTimeout.HasValue ? (int)commandTimeout.Value.TotalSeconds : (int?)null;
        }

        public IEnumerable<TSource> Query<TSource>(string sql, object param = null, bool buffered = true, TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.Query<TSource>(sql, param, _transaction, buffered, ComputeCommandTimeoutInSeconds(commandTimeout), commandType);
        }

        public Task<IEnumerable<TSource>> QueryAsync<TSource>(string sql, object param = null, TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.QueryAsync<TSource>(sql, param, _transaction, ComputeCommandTimeoutInSeconds(commandTimeout), commandType);
        }

        public int Execute(string sql, object param = null, TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.Execute(sql, param, _transaction, ComputeCommandTimeoutInSeconds(commandTimeout), commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.ExecuteAsync(sql, param, _transaction, ComputeCommandTimeoutInSeconds(commandTimeout), commandType);
        }

        public void BulkInsert<TSource>(string tableName, IReadOnlyCollection<TSource> source,
            Func<IColumnsMappingConfigurator<TSource>, IColumnsMappingConfigurator<TSource>> columnsMappingSetup = null,
            int? batchSize = null, TimeSpan? timeout = null) where TSource : class
        {
            ValidateStatus(nameof(IDbConnection));

            var sqlConnection = _connection as SqlConnection;
            var sqlTransaction = _transaction as SqlTransaction;
            if (sqlConnection == null || sqlTransaction == null)
                throw new NotImplementedException();

            sqlConnection.BulkInsert(tableName, source, columnsMappingSetup, sqlTransaction, batchSize, timeout);
        }

        public void Commit()
        {
            ValidateStatus(nameof(IDbTransaction));
            _transaction.Commit();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
            Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}