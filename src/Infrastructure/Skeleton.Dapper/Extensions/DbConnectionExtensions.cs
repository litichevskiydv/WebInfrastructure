namespace Skeleton.Dapper.Extensions
{
    using global::Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public static class DbConnectionExtensions
    {
        private static void ValidateRequiredParameters(IDbConnection dbConnection, QueryObject queryObject)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(nameof(dbConnection));
            if (queryObject == null)
                throw new ArgumentNullException(nameof(queryObject));
        }

        public static IEnumerable<TSource> Query<TSource>(this IDbConnection dbConnection, QueryObject queryObject, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(dbConnection, queryObject);

            return dbConnection.Query<TSource>(queryObject.Sql, queryObject.QueryParams, transaction, buffered, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(this IDbConnection dbConnection, QueryObject queryObject, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(dbConnection, queryObject);

            return dbConnection.QueryAsync<TSource>(queryObject.Sql, queryObject.QueryParams, transaction, commandTimeout, commandType);
        }

        public static int Execute(this IDbConnection dbConnection, QueryObject queryObject, IDbTransaction transaction = null,
           int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(dbConnection, queryObject);

            return dbConnection.Execute(queryObject.Sql, queryObject.QueryParams, transaction, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(this IDbConnection dbConnection, QueryObject queryObject, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(dbConnection, queryObject);

            return dbConnection.ExecuteAsync(queryObject.Sql, queryObject.QueryParams, transaction, commandTimeout, commandType);
        }
    }
}