namespace Skeleton.Dapper.Tvp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using global::Dapper;
    using Microsoft.SqlServer.Server;

    public static class PrimitiveValuesList
    {
        public static PrimitiveValuesList<TSource> Create<TSource>(string typeName, IEnumerable<TSource> source,
            MetaDataCreationOptions metaDataCreationOptions = null)
            where TSource : IComparable, IComparable<TSource>, IConvertible, IEquatable<TSource>
        {
            return new PrimitiveValuesList<TSource>(typeName, source, metaDataCreationOptions);
        }
    }

    public class PrimitiveValuesList<TSource> : SqlMapper.ICustomQueryParameter
        where TSource : IComparable, IComparable<TSource>, IConvertible, IEquatable<TSource>
    {
        private readonly string _typeName;
        private readonly IEnumerable<TSource> _source;
        private readonly MetaDataCreationOptions _metaDataCreationOptions;

        public PrimitiveValuesList(string typeName, IEnumerable<TSource> source, MetaDataCreationOptions metaDataCreationOptions = null)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentNullException(nameof(typeName));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _typeName = typeName;
            _source = source;
            _metaDataCreationOptions = metaDataCreationOptions;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var sqlCommand = (SqlCommand) command;
            var parameter = sqlCommand.Parameters.Add(name, SqlDbType.Structured);
            parameter.Direction = ParameterDirection.Input;
            parameter.TypeName = _typeName;

            var tvpDefinition = new[] {SqlTypesMapper.CreateSqlMetaData("Value", typeof(TSource), _metaDataCreationOptions)};
            var parameterValue = new List<SqlDataRecord>();
            foreach (var item in _source)
            {
                var record = new SqlDataRecord(tvpDefinition);
                record.SetValue(0, item);
                parameterValue.Add(record);
            }

            if (parameterValue.Count > 0)
                parameter.Value = parameterValue;
        }
    }
}