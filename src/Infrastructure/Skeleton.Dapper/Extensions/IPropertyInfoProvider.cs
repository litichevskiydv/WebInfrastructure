using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Dapper.Extensions
{
    public interface IPropertyInfoProvider
    {
        Dictionary<string, int> GetPropertiesIndicesByNames();
        int FieldCount { get; set; }
        string GetName(int ordinal);
        Type GetFieldType(int ordinal);
        string GetDataTypeName(int ordinal);
        int GetValues(object[] values);
        object GetValue(int ordinal, object collectionEnumeratorCurrent);
    }
}
