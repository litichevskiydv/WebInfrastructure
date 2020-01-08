namespace Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client.ServicesClients.ValuesService;
    using Models.Input;
    using Skeleton.Web.Integration.BaseApiClient.Exceptions;
    using Skeleton.Web.Serialization.JsonNet.Serializer;
    using Skeleton.Web.Serialization.Protobuf.Serializer;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    [Collection(nameof(ApiTestsCollection))]
    public class ValuesControllerTests : BaseServiceClientTests<Startup>
    {
        private readonly ValuesServiceClient _defaultClient;

        public ValuesControllerTests(BaseApiTestsFixture<Startup> fixture) : base(fixture)
        {
            _defaultClient = CreateClient<ValuesServiceClient, ValuesServiceClientOptions>();
        }

        [Fact]
        public void ShouldReturnValues()
        {
            Assert.NotEmpty(_defaultClient.Get());
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldReturnBadRequestIfOperationWasCancelled()
        {
            // Given
            var client = CreateClient<ValuesServiceClient, ValuesServiceClientOptions>(
                x =>
                {
                    x.Timeout = TimeSpan.FromMilliseconds(100);
                    x.Serializer = JsonNetSerializer.Default;
                }
            );

            // When, Then
            Assert.Throws<OperationCanceledException>(() => client.Get());
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyWarningWasLogged();
        }

        [Fact]
        public async Task ShouldReturnValuesAsync()
        {
            Assert.NotEmpty(await _defaultClient.GetAsync());
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
            _defaultClient.Set(
                new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}}
            );
            var actualValue = _defaultClient.Get(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldNotSetValuesBecauseOfEmptyCollection()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Set(new ValuesModificationRequest()));
        }

        [Fact]
        public void ShouldNotSetValuesBecauseOfEmptyRequest()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Set(null));
        }

        [Fact]
        public void ShouldSetValueUsingProtobufFormatters()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";


            var client =
                CreateClient<ValuesServiceClient, ValuesServiceClientOptions>(
                    x => x.Serializer = ProtobufSerializer.Default
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
            await _defaultClient.SetAsync(
                new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}}
            );
            var actualValue = await _defaultClient.GetAsync(id);

            // Then
            Assert.Equal(expectedValue, actualValue);
            Fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ShouldNotValidateNegativeKeys()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.Post(-1, "test"));

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
            await _defaultClient.PostAsync(id, expectedValue);
            var actualValue = await _defaultClient.GetAsync(id);

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
            _defaultClient.Set(
                new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "test"}}}
            );
            _defaultClient.Delete(id);

            // Then
            Assert.Throws<ApiException>(() => _defaultClient.Get(id));
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
            await _defaultClient.SetAsync(
                new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "trst"}}}
            );
            await _defaultClient.DeleteAsync(id);

            // Then
            Assert.Throws<ApiException>(() => _defaultClient.Get(id));
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
            Assert.Throws<ApiException>(() => _defaultClient.Get(id));
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
            await Assert.ThrowsAsync<ApiException>(async () => await _defaultClient.GetAsync(id));
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
            _defaultClient.Set(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "   "}}});
            await Assert.ThrowsAsync<NotFoundException>(async () => await _defaultClient.GetAsync(id));
        }
    }
}
