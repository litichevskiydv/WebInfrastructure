namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ManualConfiguredStrictTypePropertyInfoProvider<TEntity> : IPropertyInfoProvider, IColumnsMappingConfigurator<TEntity>
        where TEntity : class
    {
        private class ValuesSource
        {
            public string Name { get; }
            public Func<object, object> ValuesProvider { get; }

            public ValuesSource(string name, Func<object, object> valuesProvider)
            {
                Name = name;
                ValuesProvider = valuesProvider;
            }
        }

        private readonly SqlBulkCopyColumnMappingCollection _mappingsCollection;
        private readonly List<ValuesSource> _valuesSources;

        public ManualConfiguredStrictTypePropertyInfoProvider(SqlBulkCopyColumnMappingCollection mappingsCollection)
        {
            _mappingsCollection = mappingsCollection;

            _valuesSources = new List<ValuesSource>();
        }

        public int FieldCount => _valuesSources.Count;

        public string GetName(int ordinal)
        {
            return _valuesSources[ordinal].Name;
        }

        public object GetValue(int ordinal, object current)
        {
            return _valuesSources[ordinal].ValuesProvider(current);
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

            _valuesSources.Add(new ValuesSource(propertyInfo.Name, x => propertyInfo.GetValue(x)));
            _mappingsCollection.Add(new SqlBulkCopyColumnMapping(_valuesSources.Count - 1, tableColumnName ?? propertyInfo.Name));

            return this;
        }

        public IColumnsMappingConfigurator<TEntity> WithFunction<TValue>(
            Func<TEntity, TValue> valuesProvider, 
            string tableColumnName)
        {
            _valuesSources.Add(new ValuesSource(tableColumnName, x => valuesProvider((TEntity) x)));
            _mappingsCollection.Add(new SqlBulkCopyColumnMapping(_valuesSources.Count - 1, tableColumnName));

            return this;
        }
    }
}