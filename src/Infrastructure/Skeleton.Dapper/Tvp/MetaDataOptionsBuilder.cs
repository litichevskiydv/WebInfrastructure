namespace Skeleton.Dapper.Tvp
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal class MetaDataOptionsBuilder<TSource> : IMetaDataOptionsBuilder<TSource> where TSource : class
    {
        public Dictionary<string, MetaDataCreationOptions> PropertiesSqlMetaDataOptions { get; }

        public MetaDataOptionsBuilder()
        {
            PropertiesSqlMetaDataOptions = new Dictionary<string, MetaDataCreationOptions>();
        }

        private static string GetMemberName(LambdaExpression member)
        {
            return ((MemberExpression) member.Body).Member.Name;
        }

        private IMetaDataOptionsBuilder<TSource> SetMaxLength(LambdaExpression member, long maxLength)
        {
            if(maxLength <= 0)
                throw new ArgumentException($"{nameof(maxLength)} must be positive");

            PropertiesSqlMetaDataOptions[GetMemberName(member)] = new MetaDataCreationOptions {MaxLength = maxLength};
            return this;
        }

        public IMetaDataOptionsBuilder<TSource> SetAccuracy(Expression<Func<TSource, decimal>> member, byte precision, byte scale)
        {
            if(scale > precision)
                throw new ArgumentException($"{nameof(scale)} cannot be greater than {nameof(precision)}");

            PropertiesSqlMetaDataOptions[GetMemberName(member)] = new MetaDataCreationOptions {Precision = precision, Scale = scale};
            return this;
        }

        public IMetaDataOptionsBuilder<TSource> SetMaxLength(Expression<Func<TSource, string>> member, long maxLength)
        {
            return SetMaxLength((LambdaExpression)member, maxLength);
        }

        public IMetaDataOptionsBuilder<TSource> SetMaxLength(Expression<Func<TSource, byte[]>> member, long maxLength)
        {
            return SetMaxLength((LambdaExpression)member, maxLength);
        }
    }
}