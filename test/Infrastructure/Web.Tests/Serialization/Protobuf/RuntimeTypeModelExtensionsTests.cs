namespace Skeleton.Web.Tests.Serialization.Protobuf
{
    using System;
    using JetBrains.Annotations;
    using ProtoBuf.Meta;
    using Web.Serialization.Protobuf.Configuration;
    using Web.Serialization.Protobuf.Formatters.Surrogates;
    using Xunit;

    public class RuntimeTypeModelExtensionsTests
    {
        [UsedImplicitly]
        public static TheoryData<Func<RuntimeTypeModel, RuntimeTypeModel>> ArgumentsValidationTestCases;

        static RuntimeTypeModelExtensionsTests()
        {
            ArgumentsValidationTestCases =
                new TheoryData<Func<RuntimeTypeModel, RuntimeTypeModel>>
                {
                    x => x.WithDefaultValuesHandling(true),
                    x => x.WithDataContractsHandling(true),
                    x => x.WithDateTimeKindHandling(true),
                    x => x.WithTypeSurrogate<DateTimeOffset, DateTimeOffsetSurrogate>(),
                    x => x.WithDefaultSettings()
                };
        }


        [Theory]
        [MemberData(nameof(ArgumentsValidationTestCases))]
        public void ShouldNotValidateArguments(Func<RuntimeTypeModel, RuntimeTypeModel> call)
        {
            Assert.Throws<ArgumentNullException>(() => call(null));
        }

        [Fact]
        public void ShouldConfigureDefaultValuesHandling()
        {
            // Given
            var model = TypeModel.Create();

            // When
            model.WithDefaultValuesHandling(true);

            // Then
            Assert.True(model.UseImplicitZeroDefaults);
        }

        [Fact]
        public void ShouldConfigureDataContractsHandling()
        {
            // Given
            var model = TypeModel.Create();

            // When
            model.WithDataContractsHandling(true);

            // Then
            Assert.False(model.AutoAddProtoContractTypesOnly);
        }

        [Fact]
        public void ShouldConfigureDateTimeKindHandling()
        {
            // Given
            var model = TypeModel.Create();

            // When
            model.WithDateTimeKindHandling(true);

            // Then
            Assert.True(model.IncludeDateTimeKind);
        }
    }
}