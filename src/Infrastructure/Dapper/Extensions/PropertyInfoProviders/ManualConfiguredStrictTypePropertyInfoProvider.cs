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
        private readonly SqlBulkCopyColumnMappingCollection _mappingsCollection;

        private readonly List<PropertyInfo> _itemProperties;
        private readonly Dictionary<string, int> _propertiesIndicesByNames;

        public ManualConfiguredStrictTypePropertyInfoProvider(SqlBulkCopyColumnMappingCollection mappingsCollection)
        {
            _mappingsCollection = mappingsCollection;

            _itemProperties = new List<PropertyInfo>();
            _propertiesIndicesByNames = new Dictionary<string, int>();
        }

        public int FieldCount => _itemProperties.Count;

        public string GetName(int ordinal)
        {
            return _itemProperties[ordinal].Name;
        }

        public Type GetFieldType(int ordinal)
        {
            return _itemProperties[ordinal].PropertyType;
        }

        public int GetOrdinal(string name)
        {
            int index;
            if (_propertiesIndicesByNames.TryGetValue(name, out index) == false)
                throw new IndexOutOfRangeException();
            return index;
        }

        public object GetValue(int ordinal, object current)
        {
            return _itemProperties[ordinal].GetValue(current);
        }

        public IColumnsMappingConfigurator<TEntity> WithMapping<TProperty>(
            Expression<Func<TEntity, TProperty>> member,
            string tableColumnName = null)
        {
            if (!(member.Body is MemberExpression memberExpression))
                throw new InvalidOperationException("Member was configured uncorrectly");

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if(propertyInfo == null || propertyInfo.CanRead == false)
                throw new InvalidOperationException("Member was configured uncorrectly");

            _itemProperties.Add(propertyInfo);
            _propertiesIndicesByNames[propertyInfo.Name] = _itemProperties.Count - 1;
            _mappingsCollection.Add(new SqlBulkCopyColumnMapping(_itemProperties.Count - 1, tableColumnName ?? propertyInfo.Name));

            return this;
        }
    }
}