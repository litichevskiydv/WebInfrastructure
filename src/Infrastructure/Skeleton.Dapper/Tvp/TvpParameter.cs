namespace Skeleton.Dapper.Tvp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using global::Dapper;
    using Microsoft.SqlServer.Server;

    public static class TvpParameter
    {
        public static TvpParameter<TSource> Create<TSource>(string typeName, IEnumerable<TSource> source,
            Func<IMetaDataOptionsBuilder<TSource>, IMetaDataOptionsBuilder<TSource>> optionsConfigurator = null) where TSource : class
        {
            return new TvpParameter<TSource>(typeName, source);
        }
    }

    public class TvpParameter<TSource> : SqlMapper.ICustomQueryParameter where TSource : class
    {
        private readonly string _typeName;
        private readonly IEnumerable<TSource> _source;
        private readonly MetaDataOptionsBuilder<TSource> _optionsBuilder;

        public TvpParameter(string typeName, IEnumerable<TSource> source,
            Func<IMetaDataOptionsBuilder<TSource>, IMetaDataOptionsBuilder<TSource>> optionsConfigurator = null)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentNullException(nameof(typeName));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _typeName = typeName;
            _source = source;

            _optionsBuilder = new MetaDataOptionsBuilder<TSource>();
            optionsConfigurator?.Invoke(_optionsBuilder);
        }

        private MetaDataCreationOptions GetOptionsForColumn(string columnName)
        {
            MetaDataCreationOptions options;
            return _optionsBuilder.PropertiesSqlMetaDataOptions.TryGetValue(columnName, out options) ? options : null;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var sqlCommand = (SqlCommand)command;
            var parameter = sqlCommand.Parameters.Add(name, SqlDbType.Structured);
            parameter.Direction = ParameterDirection.Input;
            parameter.TypeName = _typeName;

            var parameterValue = new List<SqlDataRecord>();
            var itemProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var tvpDefinition = itemProperties
                .Select(x => SqlTypesMapper.CreateSqlMetaData(x.Name, x.PropertyType, GetOptionsForColumn(x.Name)))
                .ToArray();

            foreach (var item in _source)
            {
                var record = new SqlDataRecord(tvpDefinition);
                for (var i = 0; i < itemProperties.Length; i++)
                {
                    var propertyValue = itemProperties[i].GetValue(item);
                    if (propertyValue == null)
                        record.SetDBNull(i);
                    else
                        record.SetValue(i, propertyValue);
                }
                parameterValue.Add(record);
            }

            if (parameterValue.Count > 0)
                parameter.Value = parameterValue;
        }
    }
}