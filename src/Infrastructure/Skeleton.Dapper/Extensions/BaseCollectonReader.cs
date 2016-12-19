using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Dapper.Extensions
{
    public class BaseCollectonReader : DbDataReader
    {
        private readonly IEnumerator<object> _collectionEnumerator;
        private bool _disposed;

        private readonly IPropertyInfoProvider _propertyInfoProvider;

        public BaseCollectonReader(IReadOnlyCollection<object> collection, IPropertyInfoProvider propertyInfoProvider)
        {
            _propertyInfoProvider = propertyInfoProvider;

            _collectionEnumerator = collection.GetEnumerator();
            HasRows = collection.Count > 0;
        }

        public override int Depth => 0;
        public override bool IsClosed => _disposed;
        public override int RecordsAffected => 0;

        public override int FieldCount => _propertyInfoProvider.FieldCount;
        public override bool HasRows { get; }

        public override object this[string name] => GetValue(GetOrdinal(name));

        public override object this[int ordinal] => GetValue(ordinal);

        public override int GetOrdinal(string name)
        {
            return _propertyInfoProvider.GetOrdinal(name);
        }

        public override string GetName(int ordinal)
        {
            return _propertyInfoProvider.GetName(ordinal); //_itemProperties[ordinal].Name;
        }

        public override Type GetFieldType(int ordinal)
        {
            return _propertyInfoProvider.GetFieldType(ordinal);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _propertyInfoProvider.GetDataTypeName(ordinal);
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
            var valuesCount = Math.Min(values.Length, FieldCount);
            for (var i = 0; i < valuesCount; i++)
                values[i] = GetValue(i);
            return valuesCount;
        }

        public override object GetValue(int ordinal)
        {
            return _propertyInfoProvider.GetValue(ordinal, _collectionEnumerator.Current);
        }

        public override bool GetBoolean(int ordinal)
        {
            return (bool)GetValue(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return (byte)GetValue(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            return (char)GetValue(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return (DateTime)GetValue(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return (decimal)GetValue(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return (double)GetValue(ordinal);
        }

        public override string GetString(int ordinal)
        {
            return (string)GetValue(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return (long)GetValue(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return (int)GetValue(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return (short)GetValue(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return (Guid)GetValue(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return (float)GetValue(ordinal);
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
