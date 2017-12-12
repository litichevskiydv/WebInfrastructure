namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;

    public class StrictTypeMappingInfoProvider : IMappingInfoProvider
    {
        private readonly PropertyInfo[] _itemProperties;
        public StrictTypeMappingInfoProvider(Type type, SqlBulkCopyColumnMappingCollection mappingsCollection)
        {
            _itemProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (_itemProperties == null || _itemProperties.Any() == false)
                throw new InvalidOperationException("Cannot collect properties from object");

            MappingsCollection = mappingsCollection;
            for (var i = 0; i < _itemProperties.Length; i++)
                MappingsCollection.Add(new SqlBulkCopyColumnMapping(i, _itemProperties[i].Name));
        }

        public SqlBulkCopyColumnMappingCollection MappingsCollection { get; }

        public object GetValue(int ordinal, object current)
        {
            return _itemProperties[ordinal].GetValue(current);
        }
    }
}
