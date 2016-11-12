namespace Skeleton.Dapper.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    internal class CollectionReader<TSource> : DbDataReader where TSource : class
    {
        private readonly IEnumerator<TSource> _collectionEnumerator;
        private bool _disposed;

        private readonly PropertyInfo[] _itemProperties;
        private readonly Dictionary<string, int> _propertiesIndicesByNames;

        public CollectionReader(IReadOnlyCollection<TSource> collection)
        {
            _collectionEnumerator = collection.GetEnumerator();
            HasRows = collection.Count > 0;

            _itemProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertiesIndicesByNames = _itemProperties
                .Select((x, i) => new {x.Name, Index = i})
                .ToDictionary(x => x.Name, x => x.Index);
        }

        public override int Depth => 0;
        public override bool IsClosed => _disposed;
        public override int RecordsAffected => 0;

        public override int FieldCount => _itemProperties.Length;
        public override bool HasRows { get; }

        public override object this[string name] => GetValue(GetOrdinal(name));

        public override object this[int ordinal] => GetValue(ordinal);

        public override int GetOrdinal(string name)
        {
            int index;
            if (_propertiesIndicesByNames.TryGetValue(name, out index) == false)
                throw new IndexOutOfRangeException();
            return index;
        }

        public override string GetName(int ordinal)
        {
            return _itemProperties[ordinal].Name;
        }

        public override Type GetFieldType(int ordinal)
        {
            return _itemProperties[ordinal].PropertyType;
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _itemProperties[ordinal].PropertyType.Name;
        }

        public override IEnumerator GetEnumerator()
        {
            return _collectionEnumerator;
        }

        public override bool NextResult()
        {
            return _collectionEnumerator.MoveNext();
        }

        public override bool Read()
        {
            return _collectionEnumerator.MoveNext();
        }

        public override int GetValues(object[] values)
        {
            var valuesCount = Math.Min(values.Length, _itemProperties.Length);
            for (var i = 0; i < valuesCount; i++)
                values[i] = GetValue(i);
            return valuesCount;
        }

        public override object GetValue(int ordinal)
        {
            return _itemProperties[ordinal].GetValue(_collectionEnumerator.Current);
        }

        public override bool GetBoolean(int ordinal)
        {
            return (bool) GetValue(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return (byte) GetValue(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            return (char) GetValue(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return (DateTime) GetValue(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return (decimal) GetValue(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return (double) GetValue(ordinal);
        }

        public override string GetString(int ordinal)
        {
            return (string) GetValue(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return (long) GetValue(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return (int) GetValue(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return (short) GetValue(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return (Guid) GetValue(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return (float) GetValue(ordinal);
        }

        public override bool IsDBNull(int ordinal)
        {
            return GetValue(ordinal) == null;
        }

        protected override void Dispose(bool disposing)
        {
            if (IsClosed)
                return;

            if (disposing)
                _collectionEnumerator.Dispose();
            _disposed = true;
        }
    }
}