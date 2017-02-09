namespace Skeleton.Dapper.SessionsFactory
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public static class SessionExtensions
    {
        public static IEnumerable<TSource> Query<TSource>(this ISession session, QueryObject queryObject, 
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return session.Query<TSource>(queryObject.Sql, queryObject.QueryParams, buffered, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TSource>> QueryAsync<TSource>(this ISession session, QueryObject queryObject,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            return session.QueryAsync<TSource>(queryObject.Sql, queryObject.QueryParams, commandTimeout, commandType);
        }

        public static int Execute(this ISession session, QueryObject queryObject, 
            int? commandTimeout = null, CommandType? commandType = null)
        {
            return session.Execute(queryObject.Sql, queryObject.QueryParams, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(this ISession session, QueryObject queryObject, 
            int? commandTimeout = null, CommandType? commandType = null)
        {
            return session.ExecuteAsync(queryObject.Sql, queryObject.QueryParams, commandTimeout, commandType);
        }
    }
}