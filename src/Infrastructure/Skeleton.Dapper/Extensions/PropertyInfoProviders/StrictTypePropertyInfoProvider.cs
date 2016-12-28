using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    public class StrictTypePropertyInfoProvider : IPropertyInfoProvider //where TSource : class
    {
        private readonly PropertyInfo[] _itemProperties;
        private readonly Dictionary<string, int> _propertiesIndicesByNames;
        public StrictTypePropertyInfoProvider(Type type)
        {
            _itemProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (_itemProperties == null || !_itemProperties.Any())
                throw new InvalidOperationException("Cannot collect properties from object");
            _propertiesIndicesByNames = _itemProperties
                .Select((x, i) => new { x.Name, Index = i })
                .ToDictionary(x => x.Name, x => x.Index);
        }

        public int FieldCount => _itemProperties.Length;
        public string GetName(int ordinal)
        {
            return _itemProperties[ordinal].Name;
        }

        public Type GetFieldType(int ordinal)
        {
            return _itemProperties[ordinal].PropertyType;
        }

        public string GetDataTypeName(int ordinal)
        {
            return _itemProperties[ordinal].PropertyType.Name;
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
    }
}
