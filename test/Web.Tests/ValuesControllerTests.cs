namespace Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.ServicesClients;
    using Microsoft.Extensions.Options;
    using Models.Input;
    using Skeleton.Web.Integration.BaseApiClient.Exceptions;
    using Skeleton.Web.Serialization.Jil.Serializer;
    using Skeleton.Web.Serialization.JsonNet.Serializer;
    using Skeleton.Web.Serialization.Protobuf.Serializer;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    [Collection(nameof(ApiTestsCollection))]
    public class ValuesControllerTests : BaseServiceClientTests<BaseApiTestsFixture<Startup>, ValuesServiceClient>
    {
        public ValuesControllerTests(BaseApiTestsFixture<Startup> fixture)
            : base(
                fixture,
                (httpClient, baseUrl, timeout) =>
                    new ValuesServiceClient(
                        httpClient,
                        Options.Create(
                            new ValuesServiceClientOptions
                            {
                                BaseUrl = baseUrl,
                                Timeout = timeout,
                                Serializer = JilSerializer.Default
                            }
                        )
                    )
            )
        {
        }

        [Fact]
        public void ShouldReturnValues()
        {
            Assert.NotEmpty(ServiceClient.Get());
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldReturnBadRequestIfOperationWasCancelled()
        {
            // Given
            var client =
                new ValuesServiceClient(
                    Fixture.Server.CreateClient(),
                    Options.Create(
                        new ValuesServiceClientOptions
                        {
                            BaseUrl = Fixture.Server.BaseAddress.ToString(),
                            Timeout = TimeSpan.FromMilliseconds(100),
                            Serializer = JsonNetSerializer.Default
                        }
                    )
                );

            Assert.Throws<BadRequestException>(() => client.Get());
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyWarningWasLogged();
        }

        [Fact]
        public async Task ShouldReturnValuesAsync()
        {
            Assert.NotEmpty(await ServiceClient.GetAsync());
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ServiceClient.Set(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}});
            var actualValue = ServiceClient.Get(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldNotSetValuesBecauseOfEmptyCollection()
        {
            Assert.Throws<BadRequestException>(() => ServiceClient.Set(new ValuesModificationRequest()));
        }

        [Fact]
        public void ShouldNotSetValuesBecauseOfEmptyRequest()
        {
            Assert.Throws<BadRequestException>(() => ServiceClient.Set(null));
        }

        [Fact]
        public void ShouldSetValueUsingProtobufFormatters()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";


            var client =
                new ValuesServiceClient(
                    Fixture.Server.CreateClient(),
                    Options.Create(
                        new ValuesServiceClientOptions
                        {
                            BaseUrl = Fixture.Server.BaseAddress.ToString(),
                            Timeout = Fixture.ApiTimeout,
                            Serializer = ProtobufSerializer.Default
                        }
                    )
                );

            // When
            client.Set(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}});
            var actualValue = client.Get(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public async Task ShouldSetValueAsync()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await ServiceClient.SetAsync(
                new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}}
            );
            var actualValue = await ServiceClient.GetAsync(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldNotValidateNegativeKeys()
        {
            Assert.Throws<BadRequestException>(() => ServiceClient.Post(-1, "test"));

            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public async Task ShouldValidatePositiveKeys()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await ServiceClient.PostAsync(id, expectedValue);
            var actualValue = await ServiceClient.GetAsync(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldDeleteValue()
        {
            // Given
            const int id = 1;

            // When
            ServiceClient.Set(new ValuesModificationRequest { Values = new[] { new ConfigurationValue { Id = id, Value = "test" } } });
            ServiceClient.Delete(id);

            // Then
            Assert.Throws<ApiException>(() => ServiceClient.Get(id));
            Fixture.MockLogger
                .VerifyErrorWasLogged(typeof(KeyNotFoundException))
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public async Task ShouldDeleteValueAsync()
        {
            // Given
            const int id = 1;

            // When
            await ServiceClient.SetAsync(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "trst"}}});
            await ServiceClient.DeleteAsync(id);

            // Then
            Assert.Throws<ApiException>(() => ServiceClient.Get(id));
            Fixture.MockLogger
                .VerifyErrorWasLogged<KeyNotFoundException>()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldThrowExceptionWhileGettingValueByNonexistentKey()
        {
            // Given
            const int id = 2;

            // When, Then
            Assert.Throws<ApiException>(() => ServiceClient.Get(id));
            Fixture.MockLogger
                .VerifyErrorWasLogged<KeyNotFoundException>()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public async Task ShouldThrowExceptionWhileGettingValueByNonexistentKeyAsync()
        {
            // Given
            const int id = 2;

            // When, Then
            await Assert.ThrowsAsync<ApiException>(async () => await ServiceClient.GetAsync(id));
            Fixture.MockLogger
                .VerifyErrorWasLogged<KeyNotFoundException>()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public async Task ShouldReturnNotFoundAsync()
        {
            // Given
            const int id = 2;

            // When, Then
            ServiceClient.Set(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "   "}}});
            await Assert.ThrowsAsync<NotFoundException>(async () => await ServiceClient.GetAsync(id));
        }
    }
}
