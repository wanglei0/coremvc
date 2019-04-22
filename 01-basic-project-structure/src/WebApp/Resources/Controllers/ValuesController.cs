using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Resources.Repository;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUsersRepository _usersRepository;
        public ValuesController(ILogger<ValuesController> logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var guid = _usersRepository.Save();
//            _usersRepository.Delete<Users>(Guid.Parse("CBFE2138-B8BB-4F71-B1D1-4043874F15A8"));
            _logger.Log(LogLevel.Error, "here is the user id {0}", guid);

            var user = _usersRepository.Get(guid);
            _logger.Log(LogLevel.Error, "here is the user id: {0}, user firstname: {1}", user.Id, user.FirstName);
           
            _usersRepository.Delete<Users>(guid);

            _logger.Log(LogLevel.Error, "here is the log in {currentMethod}", nameof(Get));
            return new[] { "value1", "value2" };
        }
    }
}
