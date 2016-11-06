namespace Skeleton.Dapper.Tvp
{
    using System.Collections.Generic;

    public class Int64ValuesList : PrimitiveValuesList<long>
    {
        public Int64ValuesList(string typeName, IEnumerable<long> source) : base(typeName, source)
        {
        }
    }
}