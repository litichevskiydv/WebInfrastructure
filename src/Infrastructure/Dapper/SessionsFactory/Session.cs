namespace Skeleton.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
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

        public IEnumerable<TSource> Query<TSource>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.Query<TSource>(sql, param, _transaction, buffered, commandTimeout, commandType);
        }

        public Task<IEnumerable<TSource>> QueryAsync<TSource>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.QueryAsync<TSource>(sql, param, _transaction, commandTimeout, commandType);
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.Execute(sql, param, _transaction, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateStatus(nameof(IDbConnection));
            return _connection.ExecuteAsync(sql, param, _transaction, commandTimeout, commandType);
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

        ~Session()
        {
            Dispose(false);
        }
    }
}