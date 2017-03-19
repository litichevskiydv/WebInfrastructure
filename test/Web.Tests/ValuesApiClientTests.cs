namespace Web.Tests
{
    using System.Collections.Generic;
    using Client;
    using Skeleton.Web.Conventions.Responses;
    using Skeleton.Web.Integration.Exceptions;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    [Collection(nameof(ApiTestsCollection))]
    public class ValuesApiClientTests : BaseApiClientTests<BaseApiTestsFixture<TestsStartup>, ApiClient>
    {
        public ValuesApiClientTests(BaseApiTestsFixture<TestsStartup> fixture) : base(fixture)
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
            Fixture.MockLogger.VerifyNoErrors();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ApiClient
                .SetValue(id, expectedValue)
                .GetValue(id);

            // Then
            Assert.Equal(expectedValue, (string)ApiClient.CurrentState);
            Fixture.MockLogger.VerifyNoErrors();
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
            Assert.Equal(id, ((ApiResponse<int>)ApiClient.CurrentState).Data);
            Fixture.MockLogger.VerifyNoErrors();
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
                .SetValue(id, "test")
                .DeleteValue(id);

            // When, Then
            Assert.Throws<ApiException>(() => ApiClient.GetValue(id));
            Fixture.MockLogger.VerifyErrorWasLogged<KeyNotFoundException>();
        }
    }
}