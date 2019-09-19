using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly ILogger<ValuesController> logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            logger.LogWarning("We are calling {methodName}", nameof(Get));
            return new[] { "value1", "value2" };
        }
    }
}
