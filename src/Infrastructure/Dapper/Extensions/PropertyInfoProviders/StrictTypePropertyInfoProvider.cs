namespace Skeleton.Dapper.Extensions.PropertyInfoProviders
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class StrictTypePropertyInfoProvider : IPropertyInfoProvider
    {
        private readonly PropertyInfo[] _itemProperties;
        public StrictTypePropertyInfoProvider(Type type)
        {
            _itemProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (_itemProperties == null || _itemProperties.Any() == false)
                throw new InvalidOperationException("Cannot collect properties from object");
        }

        public int FieldCount => _itemProperties.Length;

        public string GetName(int ordinal)
        {
            return _itemProperties[ordinal].Name;
        }

        public object GetValue(int ordinal, object current)
        {
            return _itemProperties[ordinal].GetValue(current);
        }
    }
}
