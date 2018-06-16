namespace Web.Migrations
{
    using FluentMigrator;
    using JetBrains.Annotations;

    [UsedImplicitly]
    [Migration(201703191215)]
    public class Migration201703191215 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql("select 0");
        }
    }
}