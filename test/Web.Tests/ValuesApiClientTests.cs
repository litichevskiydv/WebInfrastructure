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
    public class ValuesApiClientTests : BaseApiClientTests<BaseApiTestsFixture<Startup>, ApiClient>
    {
        public ValuesApiClientTests(BaseApiTestsFixture<Startup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void ShouldReturnValues()
        {
            // When
            ApiClient
                .GetValues();

            // Then
            Assert.NotEmpty((IEnumerable<string>)ApiClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public async Task ShouldReturnValuesAsync()
        {
            // When
            await AsyncApiClient
                .GetValuesAsync();

            // Then
            Assert.NotEmpty((IEnumerable<string>)ApiClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ApiClient
                .SetValue(new ConfigurationValue {Id = id, Value = expectedValue})
                .GetValue(id);

            // Then
            Assert.Equal(expectedValue, (string)ApiClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public async Task ShouldSetValueAsync()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await AsyncApiClient
                .SetValueAsync(new[] {new ConfigurationValue {Id = id, Value = expectedValue}})
                .GetValueAsync(id);

            // Then
            Assert.Equal(expectedValue, (string)ApiClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public void ShouldValidatePositiveKeys()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ApiClient
                .PostValue(id, expectedValue);

            // Then
            Assert.Equal(id, ((ApiResponse<int, ApiResponseError>) ApiClient.CurrentState).Data);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public async Task ShouldValidatePositiveKeysAsync()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            await AsyncApiClient
                .PostValueAsync(id, expectedValue);

            // Then
            Assert.Equal(id, ((ApiResponse<int, ApiResponseError>) ApiClient.CurrentState).Data);
            Fixture.MockLogger.VerifyNoErrorsWasLogged();
        }

        [Fact]
        public void ShouldNotValidateNegativeKeys()
        {
            Assert.Throws<BadRequestException>(() => ApiClient.PostValue(-2, "123"));
        }

        [Fact]
        public void ShouldThrowExceptionWhileGettingValueByNonexistentKey()
        {
            Assert.Throws<ApiException>(() => ApiClient.GetValue(2));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public void ShouldDeleteValue()
        {
            // Given
            const int id = 2;

            // When
            ApiClient
                .SetValue(new ConfigurationValue {Id = id, Value = "test"})
                .DeleteValue(id);

            // When, Then
            Assert.Throws<ApiException>(() => ApiClient.GetValue(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }

        [Fact]
        public async Task ShouldDeleteValueAsync()
        {
            // Given
            const int id = 2;

            // When
            await AsyncApiClient
                .SetValueAsync(new[] {new ConfigurationValue {Id = id, Value = "test"}})
                .DeleteValueAsync(id);

            // When, Then
            await Assert.ThrowsAsync<ApiException>(async () => await AsyncApiClient.GetValueAsync(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }
    }
}