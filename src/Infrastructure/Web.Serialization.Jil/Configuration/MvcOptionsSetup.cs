namespace Skeleton.Web.Serialization.Jil.Configuration
{
    using System;
    using Formatters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly global::Jil.Options _serializerSettings;

        public MvcOptionsSetup(ILoggerFactory loggerFactory, IOptions<MvcJilOptions> jilOptions)
        {
            if(loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));
            if(jilOptions == null)
                throw new ArgumentNullException(nameof(jilOptions));

            _loggerFactory = loggerFactory;
            _serializerSettings = jilOptions.Value.SerializerSettings;
        }

        public void Configure(MvcOptions options)
        {
            options.InputFormatters.RemoveType<JsonInputFormatter>();
            options.InputFormatters.Add(new JilInputFormatter(_loggerFactory.CreateLogger<JilInputFormatter>(), _serializerSettings));

            options.OutputFormatters.RemoveType<JsonOutputFormatter>();
            options.OutputFormatters.Add(new JilOutputFormatter(_loggerFactory.CreateLogger<JilOutputFormatter>(), _serializerSettings));
        }
    }
}