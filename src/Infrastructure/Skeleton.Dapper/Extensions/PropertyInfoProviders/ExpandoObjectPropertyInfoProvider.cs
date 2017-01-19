using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    public class ExpandoObjectPropertyInfoProvider : IPropertyInfoProvider
    {
        private readonly IDictionary<string, object> _itemProperties;
        private readonly Dictionary<string, int> _propertiesIndicesByNames = new Dictionary<string, int>();
        private readonly Dictionary<int, string> _propertiesKeyIndices = new Dictionary<int, string>();

        public ExpandoObjectPropertyInfoProvider(IDictionary<string, object> properties)
        {
            if (properties == null || properties.Count == 0)
                throw new InvalidOperationException("Cannot collect properties from object");

            _itemProperties = properties;
            var tmp = _itemProperties.Keys.ToList();
            for (int i = 0; i < tmp.Count; i++)
            {
                _propertiesIndicesByNames.Add(tmp[i], i);
                _propertiesKeyIndices.Add(i, tmp[i]);
            }
        }

        public int FieldCount => _itemProperties.Count;

        public string GetName(int ordinal)
        {
            return _propertiesKeyIndices[ordinal];
        }

        public Type GetFieldType(int ordinal)
        {
            return _itemProperties[_propertiesKeyIndices[ordinal]].GetType();
        }

        public object GetValue(int ordinal, object collectionEnumeratorCurrent)
        {
            string propertyName = _propertiesKeyIndices[ordinal];
            return ((IDictionary<string, object>)collectionEnumeratorCurrent)[propertyName];
        }

        public int GetOrdinal(string name)
        {
            int index;
            if (_propertiesIndicesByNames.TryGetValue(name, out index) == false)
                throw new IndexOutOfRangeException();
            return index;
        }
    }
}
