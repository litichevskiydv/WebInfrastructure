namespace Skeleton.Dapper.Tvp
{
    using System;
    using System.Linq.Expressions;

    public interface IMetaDataOptionsBuilder<TSource> where TSource : class
    {
        IMetaDataOptionsBuilder<TSource> SetAccuracy(Expression<Func<TSource, decimal>> member, byte precision, byte scale);

        IMetaDataOptionsBuilder<TSource> SetMaxLength(Expression<Func<TSource, string>> member, long maxLength);

        IMetaDataOptionsBuilder<TSource> SetMaxLength(Expression<Func<TSource, byte[]>> member, long maxLength);
    }
}