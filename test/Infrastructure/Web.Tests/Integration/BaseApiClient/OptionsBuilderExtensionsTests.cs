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
        #region TestCases

        public class SerializerParametersValidationTestCase
        {
            public OptionsBuilder<FakeOptions> OptionsBuilder { get; set; }

            public ISerializer Serializer { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public class FakeOptions : BaseClientOptions
        {
        }

        [UsedImplicitly]
        public static TheoryData<SerializerParametersValidationTestCase> SerializerParametersValidationTestCases;

        static OptionsBuilderExtensionsTests()
        {
            SerializerParametersValidationTestCases
                = new TheoryData<SerializerParametersValidationTestCase>
                  {
                      new SerializerParametersValidationTestCase
                      {
                          Serializer = JilSerializer.Default
                      },
                      new SerializerParametersValidationTestCase
                      {
                          OptionsBuilder = new OptionsBuilder<FakeOptions>(new ServiceCollection(), "test"),
                      }
                  };
        }

        [Theory]
        [MemberData(nameof(SerializerParametersValidationTestCases))]
        public void ShouldValidateSerializerParameters(SerializerParametersValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.OptionsBuilder.WithSerializer(testCase.Serializer));
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