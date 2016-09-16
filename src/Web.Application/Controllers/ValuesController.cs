namespace Web.Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services;

    public class ValuesController : Controller
    {
        private readonly IValuesProvider _valuesProvider;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(IValuesProvider valuesProvider, ILogger<ValuesController> logger)
        {
            if(valuesProvider == null)
                throw new ArgumentNullException(nameof(valuesProvider));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _valuesProvider = valuesProvider;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Get values request");

            return _valuesProvider.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation("Get value request");

            return _valuesProvider.Get(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}