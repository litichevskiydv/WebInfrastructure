namespace Skeleton.Dapper
{
    using System;

    /// <summary>
    ///     Incapsulate SQL and Parameters for Dapper methods
    /// </summary>
    /// <remarks>
    ///     http://www.martinfowler.com/eaaCatalog/queryObject.html
    /// </remarks>
    public class QueryObject
    {
        /// <summary>
        ///     SQL string
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        ///     Parameter list
        /// </summary>
        public object QueryParams { get; private set; }

        /// <summary>
        ///     Create QueryObject for parameterized <paramref name="sql" />
        /// </summary>
        /// <param name="sql">SQL string</param>
        /// <param name="queryParams">Parameter list</param>
        public QueryObject(string sql, object queryParams = null)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException(nameof(sql));

            Sql = sql;
            QueryParams = queryParams;
        }
    }
}