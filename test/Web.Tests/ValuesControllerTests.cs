namespace Web.Tests
{
    using Client.ServicesClients;
    using Infrastructure.Web.Testing;
    using Infrastructure.Web.Testing.Extensions;
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
    }
}