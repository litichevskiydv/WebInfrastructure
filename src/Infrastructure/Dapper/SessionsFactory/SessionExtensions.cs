namespace Skeleton.Dapper.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public static class SessionExtensions
    {
        private static void ValidateRequiredParameters(ISession session, QueryObject queryObject)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (queryObject == null)
                throw new ArgumentNullException(nameof(queryObject));
        }

        public static IEnumerable<TSource> Query<TSource>(this ISession session, QueryObject queryObject, 
            bool buffered = true, TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(session, queryObject);

            return session.Query<TSource>(queryObject.Sql, queryObject.QueryParams, buffered, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(this ISession session, QueryObject queryObject,
            TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(session, queryObject);

            return session.QueryAsync<TSource>(queryObject.Sql, queryObject.QueryParams, commandTimeout, commandType);
        }

        public static int Execute(this ISession session, QueryObject queryObject, 
            TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(session, queryObject);

            return session.Execute(queryObject.Sql, queryObject.QueryParams, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(this ISession session, QueryObject queryObject, 
            TimeSpan? commandTimeout = null, CommandType? commandType = null)
        {
            ValidateRequiredParameters(session, queryObject);

            return session.ExecuteAsync(queryObject.Sql, queryObject.QueryParams, commandTimeout, commandType);
        }
    }
}