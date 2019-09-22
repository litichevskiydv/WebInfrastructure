namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using Abstractions.Configuration;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using ProcessingService;
    using Xunit;

    public class ServiceCollectionExtensionsTests
    {
        #region TestCases

        public class MessagesProcessingServiceParametersVerificationTestCase
        {
            public IServiceCollection Services { get; set; }

            public IConfiguration Configuration { get; set; }

            public Action<OptionsBuilder<NotificationsProcessingServiceOptions>> OptionsConfigurator { get; set; }
        }

        #endregion


        [UsedImplicitly]
        public static TheoryData<MessagesProcessingServiceParametersVerificationTestCase> MessagesProcessingServiceParametersVerificationTestCases;

        static ServiceCollectionExtensionsTests()
        {
            MessagesProcessingServiceParametersVerificationTestCases =
                new TheoryData<MessagesProcessingServiceParametersVerificationTestCase>
                {
                    new MessagesProcessingServiceParametersVerificationTestCase
                    {
                        Configuration = new Mock<IConfiguration>().Object,
                        OptionsConfigurator = x => { }
                    },
                    new MessagesProcessingServiceParametersVerificationTestCase
                    {
                        Services = new Mock<IServiceCollection>().Object,
                        OptionsConfigurator = x => { }
                    },
                    new MessagesProcessingServiceParametersVerificationTestCase
                    {
                        Services = new Mock<IServiceCollection>().Object,
                        Configuration = new Mock<IConfiguration>().Object
                    }
                };
        }

        [Theory]
        [MemberData(nameof(MessagesProcessingServiceParametersVerificationTestCases))]
        public void ShouldVerifyMessagesProcessingServiceParameters(MessagesProcessingServiceParametersVerificationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(
                () => testCase.Services.ConfigureMessagesProcessingService(testCase.Configuration, testCase.OptionsConfigurator)
            );
        }
    }
}