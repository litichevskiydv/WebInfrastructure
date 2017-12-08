namespace Skeleton.Dapper.TypesHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using global::Dapper;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class CollectionsHandler<TSource> : SqlMapper.ITypeHandler
    {
        private readonly bool _handleEmptyCollectionAsNull;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly Type _baseCollectionsType;

        public CollectionsHandler(bool handleEmptyCollectionAsNull, JsonSerializerSettings serializerSettings = null)
        {
            _handleEmptyCollectionAsNull = handleEmptyCollectionAsNull;
            _serializerSettings = serializerSettings
                                  ?? new JsonSerializerSettings
                                     {
                                         Converters = {new StringEnumConverter()},
                                         NullValueHandling = NullValueHandling.Ignore,
                                         DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                                         TypeNameHandling = TypeNameHandling.Auto
                                     };
            _baseCollectionsType = typeof(IReadOnlyCollection<TSource>);
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            if (value == DBNull.Value)
            {
                parameter.Value = DBNull.Value;
                return;
            }

            if (!(value is IReadOnlyCollection<TSource> collection))
                throw new ArgumentException($"Parameter type doesn't implement {nameof(IReadOnlyCollection<TSource>)}");
            if (collection.Count == 0 && _handleEmptyCollectionAsNull)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = JsonConvert.SerializeObject(value, _serializerSettings);
        }

        public object Parse(Type destinationType, object value)
        {
            if(_baseCollectionsType.IsAssignableFrom(destinationType) == false)
                throw new ArgumentException($"Parameter type doesn't implement {nameof(IReadOnlyCollection<TSource>)}");

            var collection = (IReadOnlyCollection<TSource>) JsonConvert.DeserializeObject(value as string, destinationType, _serializerSettings);
            return collection.Count == 0 && _handleEmptyCollectionAsNull ? null : collection;
        }
    }
}