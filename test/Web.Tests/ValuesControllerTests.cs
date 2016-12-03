namespace Web.Tests
{
    using Client.ServicesClients;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    public class ValuesControllerTests : BaseServiceClientTests<BaseApiTestsFixture<TestsStartup>, ValuesServiceClient>
    {
        public ValuesControllerTests(BaseApiTestsFixture<TestsStartup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void ShouldReturnValues()
        {
            Assert.NotEmpty(ServiceClient.Get());
            Fixture.Logger.VerifyNoErrors();
        }

        [Fact]
        public async void ShouldReturnValuesAsync()
        {
            Assert.NotEmpty(await ServiceClient.GetAsync());
            Fixture.Logger.VerifyNoErrors();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ServiceClient.Set(id, expectedValue);
            var actualValue = ServiceClient.Get(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.Logger.VerifyNoErrors();
        }
    }
}