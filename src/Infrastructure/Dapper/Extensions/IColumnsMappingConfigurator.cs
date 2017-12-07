namespace Skeleton.Dapper.Extensions
{
    using System;
    using System.Linq.Expressions;

    public interface IColumnsMappingConfigurator<TEntity> where TEntity : class
    {
        IColumnsMappingConfigurator<TEntity> WithProperty<TProperty>(
            Expression<Func<TEntity, TProperty>> member,
            string tableColumnName = null);

        IColumnsMappingConfigurator<TEntity> WithFunction<TValue>(
            Func<TEntity, TValue> valuesProvider,
            string tableColumnName);
    }
}