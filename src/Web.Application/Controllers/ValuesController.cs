namespace Web.Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using Domain.CommandContexts;
    using Domain.Criteria;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services;
    using Skeleton.CQRS.Abstractions.Commands;
    using Skeleton.CQRS.Abstractions.Queries;

    /// <summary>
    /// Endpoint for configuration values
    /// </summary>
    public class ValuesController : Controller
    {
        private readonly IQueriesDispatcher _queriesDispatcher;
        private readonly ICommandsDispatcher _commandsDispatcher;
        private readonly IValuesProvider _valuesProvider;
        private readonly ILogger<ValuesController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queriesDispatcher">Queries dispatcher</param>
        /// <param name="commandsDispatcher">Commands dispatcher</param>
        /// <param name="valuesProvider">Configuration values provider</param>
        /// <param name="logger">Messages logger</param>
        public ValuesController(IQueriesDispatcher queriesDispatcher, ICommandsDispatcher commandsDispatcher, IValuesProvider valuesProvider,
            ILogger<ValuesController> logger)
        {
            if(queriesDispatcher == null)
                throw new ArgumentNullException(nameof(queriesDispatcher));
            if (commandsDispatcher == null)
                throw new ArgumentNullException(nameof(commandsDispatcher));
            if (valuesProvider == null)
                throw new ArgumentNullException(nameof(valuesProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _queriesDispatcher = queriesDispatcher;
            _commandsDispatcher = commandsDispatcher;
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
            return _queriesDispatcher.Execute<string>(new GetValueQueryCriterion(id));
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
            _logger.LogInformation("Set value request");
            _commandsDispatcher.Execute(new SetValueCommandContext(id, value));
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