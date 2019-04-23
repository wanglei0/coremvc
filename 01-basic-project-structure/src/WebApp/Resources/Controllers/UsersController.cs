using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Resources.Repository;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<Users> repo;

        public UsersController(ILogger<ValuesController> logger, IBaseRepository<Users> repo)
        {
            _logger = logger;
            repo = repo;
        }

        [HttpGet]
        public ActionResult Save()
        {
            Request.Body.
        }
    }
}