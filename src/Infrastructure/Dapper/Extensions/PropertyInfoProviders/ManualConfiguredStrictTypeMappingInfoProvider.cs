namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ManualConfiguredStrictTypeMappingInfoProvider<TEntity> : IMappingInfoProvider, IColumnsMappingConfigurator<TEntity>
        where TEntity : class
    {
        private readonly List<Func<object, object>> _valuesSources;

        public ManualConfiguredStrictTypeMappingInfoProvider(SqlBulkCopyColumnMappingCollection mappingsCollection)
        {
            _valuesSources = new List<Func<object, object>>();
            MappingsCollection = mappingsCollection;
        }

        public SqlBulkCopyColumnMappingCollection MappingsCollection { get; }

        public object GetValue(int ordinal, object current)
        {
            return _valuesSources[ordinal](current);
        }

        public IColumnsMappingConfigurator<TEntity> WithProperty<TProperty>(
            Expression<Func<TEntity, TProperty>> member,
            string tableColumnName = null)
        {
            if (!(member.Body is MemberExpression memberExpression))
                throw new InvalidOperationException("Member was configured uncorrectly");

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if(propertyInfo == null || propertyInfo.CanRead == false)
                throw new InvalidOperationException("Member was configured uncorrectly");

            _valuesSources.Add(x => propertyInfo.GetValue(x));
            MappingsCollection.Add(new SqlBulkCopyColumnMapping(_valuesSources.Count - 1, tableColumnName ?? propertyInfo.Name));

            return this;
        }

        public IColumnsMappingConfigurator<TEntity> WithFunction<TValue>(
            Func<TEntity, TValue> valuesProvider, 
            string tableColumnName)
        {
            _valuesSources.Add(x => valuesProvider((TEntity) x));
            MappingsCollection.Add(new SqlBulkCopyColumnMapping(_valuesSources.Count - 1, tableColumnName));

            return this;
        }
    }
}