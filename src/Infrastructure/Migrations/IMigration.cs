namespace Skeleton.Migrations
{
    public interface IMigration
    {
        long Version { get; }

        string SqlSourceCode { get; }
    }
}