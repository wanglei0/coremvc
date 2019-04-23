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
//        private readonly IBaseRepository<Users> repo;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
//            repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
//            var user = repo.Insert(user);
////            _usersRepository.Delete<Users>(Guid.Parse("CBFE2138-B8BB-4F71-B1D1-4043874F15A8"));
//            _logger.Log(LogLevel.Error, "here is the user id {0}", user);
//
//            var user = repo.GetById(user.Id);
//            _logger.Log(LogLevel.Error, "here is the user id: {0}, user firstname: {1}", user.Id, user.FirstName);
//           
//            repo.Delete(user.Id);

            _logger.Log(LogLevel.Error, "here is the log in {currentMethod}", nameof(Get));
            return new[] { "value1", "value2" };
        }
    }
}
