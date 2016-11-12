namespace Skeleton.Dapper.ConnectionsFactory
{
    using System.Data;

    public interface IConnectionsFactory
    {
        IDbConnection Create();
    }
}