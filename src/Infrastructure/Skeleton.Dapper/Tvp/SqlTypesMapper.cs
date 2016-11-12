namespace Skeleton.Dapper.Tvp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.SqlServer.Server;

    public static class SqlTypesMapper
    {
        private static readonly Dictionary<Type, SqlDbType> SqlTypesMappings;
        private static readonly Dictionary<Type, Func<string, MetaDataCreationOptions, SqlMetaData>> SqlMetaDataCreators;

        private static readonly MetaDataCreationOptions DefaultArraysMetaDataCreationOptions;
        private static readonly MetaDataCreationOptions DefaultDecimalMetaDataCreationOptions;

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
        private static SqlMetaData CreatorForScalarTypes(string columnName, SqlDbType sqlDbType, MetaDataCreationOptions metaDataCreationOptions)
        {
            return new SqlMetaData(columnName, sqlDbType);
        }

        private static SqlMetaData CreatorForChar(string columnName, SqlDbType sqlDbType, MetaDataCreationOptions metaDataCreationOptions)
        {
            return new SqlMetaData(columnName, sqlDbType, 1);
        }

        private static SqlMetaData CreatorForDecimal(string columnName, SqlDbType sqlDbType, MetaDataCreationOptions metaDataCreationOptions)
        {
            if (metaDataCreationOptions != null && metaDataCreationOptions.Precision.HasValue == false)
                throw new ArgumentNullException(nameof(metaDataCreationOptions.Precision));
            if (metaDataCreationOptions != null && metaDataCreationOptions.Scale.HasValue == false)
                throw new ArgumentNullException(nameof(metaDataCreationOptions.Scale));
            var options = metaDataCreationOptions ?? DefaultDecimalMetaDataCreationOptions;

            return new SqlMetaData(columnName, sqlDbType, options.Precision.Value, options.Scale.Value);
        }

        private static SqlMetaData CreatorForArrays(string columnName, SqlDbType sqlDbType, MetaDataCreationOptions metaDataCreationOptions)
        {
            if (metaDataCreationOptions != null && metaDataCreationOptions.MaxLength.HasValue == false)
                throw new ArgumentNullException(nameof(metaDataCreationOptions.MaxLength));

            return new SqlMetaData(columnName, sqlDbType, (metaDataCreationOptions ?? DefaultArraysMetaDataCreationOptions).MaxLength.Value);
        }

        private static SqlMetaData CreatorForObject(string columnName, SqlDbType sqlDbType, MetaDataCreationOptions metaDataCreationOptions)
        {
            return new SqlMetaData(columnName, sqlDbType);
        }

        private static Dictionary<Type, Func<string, MetaDataCreationOptions, SqlMetaData>> AddCreator<TValue>(
            this Dictionary<Type, Func<string, MetaDataCreationOptions, SqlMetaData>> mappings,
            Func<string, SqlDbType, MetaDataCreationOptions, SqlMetaData> creator)
        {
            var type = typeof(TValue);
            mappings[type] = (columnName, options) => creator(columnName, GetSqlType(type), options);

            if (default(TValue) != null)
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(type);
                mappings[nullableType] = (columnName, options) => creator(columnName, GetSqlType(nullableType), options);
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

            DefaultArraysMetaDataCreationOptions = new MetaDataCreationOptions {MaxLength = -1};
            DefaultDecimalMetaDataCreationOptions = new MetaDataCreationOptions {Precision = 10, Scale = 2};
            SqlMetaDataCreators = new Dictionary<Type, Func<string, MetaDataCreationOptions, SqlMetaData>>()
                .AddCreator<byte>(CreatorForScalarTypes)
                .AddCreator<short>(CreatorForScalarTypes)
                .AddCreator<char>(CreatorForChar)
                .AddCreator<int>(CreatorForScalarTypes)
                .AddCreator<long>(CreatorForScalarTypes)
                .AddCreator<float>(CreatorForScalarTypes)
                .AddCreator<double>(CreatorForScalarTypes)
                .AddCreator<decimal>(CreatorForDecimal)
                .AddCreator<bool>(CreatorForScalarTypes)
                .AddCreator<Guid>(CreatorForScalarTypes)
                .AddCreator<DateTime>(CreatorForScalarTypes)
                .AddCreator<DateTimeOffset>(CreatorForScalarTypes)
                .AddCreator<byte[]>(CreatorForArrays)
                .AddCreator<string>(CreatorForArrays)
                .AddCreator<object>(CreatorForObject);
        }

        public static SqlDbType GetSqlType(Type type)
        {
            SqlDbType dbType;
            if (SqlTypesMappings.TryGetValue(type, out dbType) == false)
                throw new InvalidOperationException($"No SqlDbType mapping was found for {type}");

            return dbType;
        }

        public static SqlMetaData CreateSqlMetaData(string columnName, Type type, MetaDataCreationOptions metaDataCreationOptions)
        {
            Func<string, MetaDataCreationOptions, SqlMetaData> creator;
            if (SqlMetaDataCreators.TryGetValue(type, out creator) == false)
                throw new InvalidOperationException($"No SqlMetaData was found for {type}");

            return creator(columnName, metaDataCreationOptions);
        }
    }
}