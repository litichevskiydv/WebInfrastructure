namespace Web.Application.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;

    [UsedImplicitly]
    public class SimpleValuesProviderConfiguration
    {
        public string[] Values { get; [UsedImplicitly] set; }

        public string Value { get; [UsedImplicitly] set; }
    }

    [UsedImplicitly]
    public class SimpleValuesProvider : IValuesProvider
    {
        private readonly SimpleValuesProviderConfiguration _configuration;

        public SimpleValuesProvider(IOptions<SimpleValuesProviderConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration.Value;
        }

        public IEnumerable<string> Get()
        {
            return _configuration.Values;
        }

        public string Get(int id)
        {
            return _configuration.Value;
        }
    }
}