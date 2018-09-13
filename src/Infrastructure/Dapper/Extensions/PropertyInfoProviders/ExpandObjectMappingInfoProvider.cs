namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class ExpandoObjectMappingInfoProvider : IMappingInfoProvider
    {
        private readonly Dictionary<int, string> _propertiesKeyIndices;

        public ExpandoObjectMappingInfoProvider(
            IDictionary<string, object> properties,
            SqlBulkCopyColumnMappingCollection mappingsCollection)
        {
            if (properties == null || properties.Count == 0)
                throw new InvalidOperationException("Cannot collect properties from object");

            MappingsCollection = mappingsCollection;
            _propertiesKeyIndices = new Dictionary<int, string>();

            foreach (var property in properties)
            {
                var ordinal = _propertiesKeyIndices.Count;

                _propertiesKeyIndices.Add(ordinal, property.Key);
                MappingsCollection.Add(new SqlBulkCopyColumnMapping(ordinal, property.Key));
            }
        }

        public SqlBulkCopyColumnMappingCollection MappingsCollection { get; }

        public object GetValue(int ordinal, object collectionEnumeratorCurrent)
        {
            var propertyName = _propertiesKeyIndices[ordinal];
            return ((IDictionary<string, object>)collectionEnumeratorCurrent)[propertyName];
        }
    }
}
