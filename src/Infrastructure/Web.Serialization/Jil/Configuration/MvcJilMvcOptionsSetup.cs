namespace Skeleton.Web.Serialization.Jil.Configuration
{
    using System;
    using Formatters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MvcJilMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly ILoggerFactory _loggerFactory;

        public MvcJilMvcOptionsSetup(ILoggerFactory loggerFactory)
        {
            if(loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _loggerFactory = loggerFactory;
        }

        public void Configure(MvcOptions options)
        {
            options.InputFormatters.RemoveType<JsonInputFormatter>();
            options.InputFormatters.Add(new JilInputFormatter(_loggerFactory.CreateLogger<JilInputFormatter>()));

            options.OutputFormatters.RemoveType<JsonOutputFormatter>();
            options.OutputFormatters.Add(new JilOutputFormatter());
        }
    }
}