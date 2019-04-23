using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models.Dtos;
using WebApp.Resources.Repository;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<Users> _repo;

        public UsersController(ILogger<ValuesController> logger, IBaseRepository<Users> repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpPost("create")]
        public ActionResult CreateUser([FromBody] UsersDto user)
        {
            var tempUser = new Users(user.FirstName, user.LastName);

            var result = _repo.Insert(tempUser);

            return Ok(result.LastName);
        }
    }
}