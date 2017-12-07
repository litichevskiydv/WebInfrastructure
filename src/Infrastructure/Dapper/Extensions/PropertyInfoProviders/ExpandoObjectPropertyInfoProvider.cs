using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    public class ExpandoObjectPropertyInfoProvider : IPropertyInfoProvider
    {
        private readonly IDictionary<string, object> _itemProperties;
        private readonly Dictionary<int, string> _propertiesKeyIndices = new Dictionary<int, string>();

        public ExpandoObjectPropertyInfoProvider(IDictionary<string, object> properties)
        {
            if (properties == null || properties.Count == 0)
                throw new InvalidOperationException("Cannot collect properties from object");

            _itemProperties = properties;
            var tmp = _itemProperties.Keys.ToList();
            for (var i = 0; i < tmp.Count; i++)
                _propertiesKeyIndices.Add(i, tmp[i]);
        }

        public int FieldCount => _itemProperties.Count;

        public string GetName(int ordinal)
        {
            return _propertiesKeyIndices[ordinal];
        }

        public object GetValue(int ordinal, object collectionEnumeratorCurrent)
        {
            var propertyName = _propertiesKeyIndices[ordinal];
            return ((IDictionary<string, object>)collectionEnumeratorCurrent)[propertyName];
        }
    }
}
