using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Resources;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ValuesController(ILogger<ObjectResult> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            using (var session = FluentNHibernateHelper.OpenSession())

            {

                var product = new Users { Id = 1, FirstName = "firstname", LastName = "lastname" };

                session.SaveOrUpdate(product);
                session.Flush();

            }
            _logger.Log(LogLevel.Error, "here is the log in {currentMethod}", nameof(Get));
            return new[] { "value1", "value2" };
        }
    }
}
