namespace Skeleton.Dapper.Tvp
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class SqlTypesMapper
    {
        private static readonly Dictionary<Type, SqlDbType> SqlTypesMappings;

        private static Dictionary<Type, SqlDbType> AddMap<TValue>(this Dictionary<Type, SqlDbType> mappings, SqlDbType sqlDbType)
        {
            var type = typeof(TValue);
            mappings[type] = sqlDbType;

            if (default(TValue) != null)
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(type);
                mappings[nullableType] = sqlDbType;
            }
            return mappings;
        }

        static SqlTypesMapper()
        {
            SqlTypesMappings = new Dictionary<Type, SqlDbType>()
                .AddMap<byte>(SqlDbType.TinyInt)
                .AddMap<short>(SqlDbType.SmallInt)
                .AddMap<char>(SqlDbType.NChar)
                .AddMap<int>(SqlDbType.Int)
                .AddMap<long>(SqlDbType.BigInt)
                .AddMap<float>(SqlDbType.Real)
                .AddMap<double>(SqlDbType.Float)
                .AddMap<decimal>(SqlDbType.Decimal)
                .AddMap<bool>(SqlDbType.Bit)
                .AddMap<Guid>(SqlDbType.UniqueIdentifier)
                .AddMap<DateTime>(SqlDbType.DateTime)
                .AddMap<DateTimeOffset>(SqlDbType.DateTimeOffset)
                .AddMap<TimeSpan>(SqlDbType.Time)
                .AddMap<byte[]>(SqlDbType.VarBinary)
                .AddMap<char[]>(SqlDbType.NVarChar)
                .AddMap<string>(SqlDbType.NVarChar)
                .AddMap<object>(SqlDbType.Variant);
        }

        public static SqlDbType GetSqlType(Type type)
        {
            return SqlTypesMappings[type];
        }
    }
}