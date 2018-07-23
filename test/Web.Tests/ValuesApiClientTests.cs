namespace Web.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client;
    using Models.Input;
    using Skeleton.Web.Conventions.Responses;
    using Skeleton.Web.Integration.BaseApiClient.Exceptions;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    [Collection(nameof(ApiTestsCollection))]
    public class ValuesApiClientTests : BaseApiClientTests<Startup>
    {
        private readonly ApiClient _defaultClient;
        private readonly dynamic _defaultAsyncClient;

        public ValuesApiClientTests(BaseApiTestsFixture<Startup> fixture) : base(fixture)
        {
            _defaultClient = CreateClient<ApiClient, ApiClientOptions>();
            _defaultAsyncClient = CreateAsyncClient(_defaultClient);
        }

        [Fact]
        public void ShouldReturnValues()
        {
            // When
            _defaultClient
                .GetValues();

            // Then
            Assert.NotEmpty((IEnumerable<string>)_defaultClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public async Task ShouldReturnValuesAsync()
        {
            // When
            await _defaultClient
                .GetValuesAsync();

            // Then
            Assert.NotEmpty((IEnumerable<string>)_defaultClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            _defaultClient
                .SetValue(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}})
                .GetValue(id);

            // Then
            Assert.Equal(expectedValue, (string)_defaultClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public async Task ShouldSetValueAsync()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await _defaultAsyncClient
                .SetValueAsync(
                    new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = expectedValue}}}
                )
                .GetValueAsync(id);

            // Then
            Assert.Equal(expectedValue, (string)_defaultClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public void ShouldValidatePositiveKeys()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            _defaultClient
                .PostValue(id, expectedValue);

            // Then
            Assert.Equal(id, ((ApiResponse<int, ApiResponseError>)_defaultClient.CurrentState).Data);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public async Task ShouldValidatePositiveKeysAsync()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await _defaultAsyncClient
                .PostValueAsync(id, expectedValue);

            // Then
            Assert.Equal(id, ((ApiResponse<int, ApiResponseError>)_defaultClient.CurrentState).Data);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public void ShouldNotValidateNegativeKeys()
        {
            Assert.Throws<BadRequestException>(() => _defaultClient.PostValue(-2, "123"));
        }

        [Fact]
        public void ShouldThrowExceptionWhileGettingValueByNonexistentKey()
        {
            Assert.Throws<ApiException>(() => _defaultClient.GetValue(2));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public void ShouldDeleteValue()
        {
            // Given
            const int id = 2;

            // When
            _defaultClient
                .SetValue(new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "test"}}})
                .DeleteValue(id);

            // When, Then
            Assert.Throws<ApiException>(() => _defaultClient.GetValue(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public async Task ShouldDeleteValueAsync()
        {
            // Given
            const int id = 2;

            // When
            await _defaultAsyncClient
                .SetValueAsync(
                    new ValuesModificationRequest {Values = new[] {new ConfigurationValue {Id = id, Value = "test"}}}
                )
                .DeleteValueAsync(id);

            // When, Then
            await Assert.ThrowsAsync<ApiException>(async () => await _defaultAsyncClient.GetValueAsync(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }
    }
}