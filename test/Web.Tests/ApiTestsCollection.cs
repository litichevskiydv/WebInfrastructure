namespace Web.Tests
{
    using Skeleton.Web.Testing;
    using Xunit;

    [CollectionDefinition(nameof(ApiTestsCollection))]
    public class ApiTestsCollection : ICollectionFixture<BaseApiTestsFixture<Startup>>
    {
    }
}