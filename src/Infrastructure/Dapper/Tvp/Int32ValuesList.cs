namespace Skeleton.Dapper.Tvp
{
    using System.Collections.Generic;

    public class Int32ValuesList : PrimitiveValuesList<int>
    {
        public Int32ValuesList(string typeName, IEnumerable<int> source) : base(typeName, source)
        {
        }
    }
}