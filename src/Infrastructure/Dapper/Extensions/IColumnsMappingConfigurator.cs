namespace Skeleton.Dapper.Extensions
{
    using System;
    using System.Linq.Expressions;

    public interface IColumnsMappingConfigurator<TEntity> where TEntity : class
    {
        IColumnsMappingConfigurator<TEntity> WithMapping<TProperty>(
            Expression<Func<TEntity, TProperty>> member,
            string tableColumnName = null);
    }
}