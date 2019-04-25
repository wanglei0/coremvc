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
        private readonly IBaseRepository<User> _repo;
        private User user;

        public UsersController(ILogger<ValuesController> logger, IBaseRepository<User> repo, User user)
        {
            _logger = logger;
            _repo = repo;
            this.user = user;
        }

        [HttpPost("create")]
        public ActionResult CreateUser([FromBody] UsersDto user)
        {
            this.user.Set(user.FirstName, user.LastName);

            var result = _repo.Insert(this.user);

            var u = _repo.GetById(Guid.Parse("C60A210D-E50A-4CFD-8DAE-2732600AD488"));
            return Ok(u);
        }
    }
}