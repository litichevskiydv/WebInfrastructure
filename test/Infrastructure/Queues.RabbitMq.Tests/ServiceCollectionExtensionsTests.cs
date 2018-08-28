namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using System.Collections.Generic;
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
        [UsedImplicitly]
        public static IEnumerable<object[]> ConfigureMessagesProcessingServiceValidationTestsData;

        static ServiceCollectionExtensionsTests()
        {
            ConfigureMessagesProcessingServiceValidationTestsData =
                new[]
                {
                    new object[]
                    {
                        null,
                        new Mock<IConfiguration>().Object,
                        new Action<OptionsBuilder<NotificationsProcessingServiceOptions>>(x => { })
                    },
                    new object[]
                    {
                        new Mock<IServiceCollection>().Object, 
                        null,
                        new Action<OptionsBuilder<NotificationsProcessingServiceOptions>>(x => { })
                    },
                    new object[]
                    {
                        new Mock<IServiceCollection>().Object,
                        new Mock<IConfiguration>().Object,
                        null
                    },
                };
        }

        [Theory]
        [MemberData(nameof(ConfigureMessagesProcessingServiceValidationTestsData))]
        public void ConfigureMessagesProcessingServiceShouldValidateParameters(
            IServiceCollection services,
            IConfiguration configuration,
            Action<OptionsBuilder<NotificationsProcessingServiceOptions>> optionsConfigurator)
        {
            Assert.Throws<ArgumentNullException>(() => services.ConfigureMessagesProcessingService(configuration, optionsConfigurator));
        }
    }
}