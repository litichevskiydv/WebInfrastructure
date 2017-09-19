namespace Skeleton.Web.Tests
{
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Serialization.JsonNet;
    using Serialization.JsonNet.PropertyNamesResolvers;
    using Xunit;

    public class SnakeCasePropertyNamesContractResolverTests
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private class Request
        {
            public string RepoName { get; set; }

            public string Data { get; set; }

            protected bool Equals(Request other)
            {
                return string.Equals(RepoName, other.RepoName) && string.Equals(Data, other.Data);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Request) obj);
            }
        }

        private readonly JsonSerializerSettings _serializerSettings;

        public SnakeCasePropertyNamesContractResolverTests()
        {
            _serializerSettings = new JsonSerializerSettings().UseContractResolver(new SnakeCasePropertyNamesContractResolver());
        }

        [Fact]
        public void ShouldDeserializeData()
        {
            // Given
            const string requestString = @"{ ""repo_name"":""SomeName"", ""data"":""SomeData"" }";
            var expectedRequest = new Request {RepoName = "SomeName", Data = "SomeData"};

            // When
            var actualRequest = JsonConvert.DeserializeObject<Request>(requestString, _serializerSettings);

            // Then
            Assert.Equal(expectedRequest, actualRequest);
        }
    }
}