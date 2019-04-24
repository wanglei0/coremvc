using System;
using System.Collections.Generic;
using System.Linq;
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
        private Users _user;

        public UsersController(ILogger<ValuesController> logger, IBaseRepository<Users> repo, Users user)
        {
            _logger = logger;
            _repo = repo;
            _user = user;
        }

        [HttpPost("create")]
        public ActionResult CreateUser([FromBody] UsersDto user)
        {
            _user.Set(user.FirstName, user.LastName);

            var result = _repo.Insert(_user);

            var u = _repo.GetById(Guid.Parse("76924659-EFFE-4E1B-A415-2E9965FD771A"));
            return Ok(u);
        }
    }
}