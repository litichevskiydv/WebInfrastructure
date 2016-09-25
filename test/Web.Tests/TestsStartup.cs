namespace Web.Tests
{
    using Infrastructure.Web.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class TestsStartup : Startup
    {
        protected override IConfigurationBuilder AddAdditionalConfigurations(IHostingEnvironment env, IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder;
        }

        protected override void AddLoggerProviders(ILoggerFactory loggerFactory)
        {
        }

        public TestsStartup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
            : base(env, commandLineArgumentsProvider)
        {
        }
    }
}