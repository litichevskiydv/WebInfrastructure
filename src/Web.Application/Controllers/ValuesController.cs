namespace Web.Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services;

    /// <summary>
    /// Endpoint for configuration values
    /// </summary>
    public class ValuesController : Controller
    {
        private readonly IValuesProvider _valuesProvider;
        private readonly ILogger<ValuesController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valuesProvider">Configuration values provider</param>
        /// <param name="logger">Messages logger</param>
        public ValuesController(IValuesProvider valuesProvider, ILogger<ValuesController> logger)
        {
            if(valuesProvider == null)
                throw new ArgumentNullException(nameof(valuesProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _valuesProvider = valuesProvider;
            _logger = logger;
        }

        /// <summary>
        /// Provides values array
        /// </summary>
        /// <returns>Configuration values</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Get values request");

            return _valuesProvider.Get();
        }

        /// <summary>
        /// Provide configuration value
        /// </summary>
        /// <param name="id">Value index</param>
        /// <returns>Configuration value</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation("Get value request");

            return _valuesProvider.Get(id);
        }

        /// <summary>
        /// Dummy post configuration value to config
        /// </summary>
        /// <param name="value">New value</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// Dummy put configuration value to config
        /// </summary>
        /// <param name="id">Configuration value index</param>
        /// <param name="value">New value</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// Dummy delete configuration value
        /// </summary>
        /// <param name="id">Configuration value index</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}