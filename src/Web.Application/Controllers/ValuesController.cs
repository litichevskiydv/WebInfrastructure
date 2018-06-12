namespace Web.Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.CommandContexts;
    using Domain.Criteria;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Input;
    using Skeleton.CQRS.Abstractions.Commands;
    using Skeleton.CQRS.Abstractions.Queries;
    using Skeleton.Web.Conventions.Responses;

    /// <summary>
    /// Endpoint for configuration values
    /// </summary>
    public class ValuesController : Controller
    {
        private readonly IQueriesDispatcher _queriesDispatcher;
        private readonly ICommandsDispatcher _commandsDispatcher;
        private readonly ILogger<ValuesController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queriesDispatcher">Queries dispatcher</param>
        /// <param name="commandsDispatcher">Commands dispatcher</param>
        /// <param name="logger">Messages logger</param>
        public ValuesController(IQueriesDispatcher queriesDispatcher, ICommandsDispatcher commandsDispatcher, ILogger<ValuesController> logger)
        {
            if(queriesDispatcher == null)
                throw new ArgumentNullException(nameof(queriesDispatcher));
            if (commandsDispatcher == null)
                throw new ArgumentNullException(nameof(commandsDispatcher));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _queriesDispatcher = queriesDispatcher;
            _commandsDispatcher = commandsDispatcher;
            _logger = logger;
        }

        /// <summary>
        /// Provides values array
        /// </summary>
        /// <returns>Configuration values</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get values request");
            await Task.Delay(500, cancellationToken);
            return await _queriesDispatcher.ExecuteAsync(new GetAllValuesQueryCriterion());
        }

        /// <summary>
        /// Provide configuration value
        /// </summary>
        /// <param name="id">Value index</param>
        /// <returns>Configuration value</returns>
        [HttpGet("{id}")]
        [Produces(typeof(string))]
        public IActionResult Get(int id)
        {
            _logger.LogInformation("Get value request");
            var value = _queriesDispatcher.Execute(new GetValueQueryCriterion(id));
            return string.IsNullOrWhiteSpace(value) ? (IActionResult) NotFound() : Ok(value);
        }

        /// <summary>
        /// Dummy validate and post configuration value to config
        /// </summary>
        /// <param name="id">Configuration value index</param>
        /// <param name="value">New value</param>
        [HttpPost("{id}")]
        [Produces(typeof(ApiResponse<int, ApiResponseError>))]
        public IActionResult Post(int id, [FromBody] string value)
        {
            _logger.LogInformation("Validation and post value request");

            if (id < 0)
                return BadRequest(ApiResponse.Error(new[] {new ApiResponseError {Code = "01", Title = "Id shouldn't be negative"}}));

            _commandsDispatcher.Execute(new SetValueCommandContext(id, value));
            return Ok(ApiResponse.Success(id));
        }

        /// <summary>
        /// Dummy put configuration values to config
        /// </summary>
        /// <param name="request">Request for values modification</param>
        [HttpPut]
        public void Put([FromBody] [Required] ValuesModificationRequest request)
        {
            _logger.LogInformation("Set values request");
            foreach (var configurationValue in request.Values)
                _commandsDispatcher.Execute(new SetValueCommandContext(configurationValue.Id, configurationValue.Value));
        }

        /// <summary>
        /// Dummy delete configuration value
        /// </summary>
        /// <param name="id">Configuration value index</param>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            _logger.LogInformation("Delete value request");
            await _commandsDispatcher.ExecuteAsync(new DeleteValueAsyncCommandContext(id));
        }
    }
}