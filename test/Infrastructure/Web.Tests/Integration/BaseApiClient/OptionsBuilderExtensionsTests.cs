namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Web.Integration.BaseApiClient.Configuration;
    using Web.Serialization.Abstractions;
    using Web.Serialization.Jil.Serializer;
    using Xunit;

    public class OptionsBuilderExtensionsTests
    {
        [UsedImplicitly]
        public class FakeOptions : BaseClientOptions
        {
        }

        [UsedImplicitly]
        public static IEnumerable<object[]> WithSerializerValidationTestsData;

        static OptionsBuilderExtensionsTests()
        {
            WithSerializerValidationTestsData
                = new[]
                  {
                      new object[] {null, JilSerializer.Default},
                      new object[] { new OptionsBuilder<FakeOptions>(new ServiceCollection(), "test"), null},
                  };
        }

        [Theory]
        [MemberData(nameof(WithSerializerValidationTestsData))]
        public void WithSerializerShouldValidateParameters(OptionsBuilder<FakeOptions> optionsBuilder, ISerializer serializer)
        {
            Assert.Throws<ArgumentNullException>(() => optionsBuilder.WithSerializer(serializer));
        }

        [Fact]
        public void ShouldConfigureSerializer()
        {
            // Given
            var expectedSerializer = JilSerializer.Default;
            var serviceProvider = new ServiceCollection()
                .AddOptions<FakeOptions>()
                .WithSerializer(expectedSerializer)
                .Services.BuildServiceProvider();

            // When
            var fakeOptions = serviceProvider.GetService<IOptions<FakeOptions>>();

            // Then
            Assert.Equal(expectedSerializer, fakeOptions.Value.Serializer);
        }
    }
}