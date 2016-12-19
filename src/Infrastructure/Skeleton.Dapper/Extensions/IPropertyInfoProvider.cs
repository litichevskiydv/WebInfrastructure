using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Dapper.Extensions
{
    public interface IPropertyInfoProvider
    {
        int FieldCount { get; }
        string GetName(int ordinal);
        Type GetFieldType(int ordinal);
        string GetDataTypeName(int ordinal);
        int GetOrdinal(string name);
        object GetValue(int ordinal, object current);
    }
}
