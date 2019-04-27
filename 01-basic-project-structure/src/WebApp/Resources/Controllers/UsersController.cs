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
        private User _user;

        public UsersController(ILogger<ValuesController> logger, IBaseRepository<User> repo, User user)
        {
            _logger = logger;
            _repo = repo;
            _user = user;
        }

        [HttpPost("create")]
        public ActionResult CreateUser([FromBody] UserDto user)
        {
            
            if (user.LastName.Length > 50 || user.FirstName.Length > 50)
            {
                return BadRequest("User name too long");
            }
            
            _user.Set(user.FirstName, user.LastName);
            var result = _repo.Insert(_user);
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public ActionResult GetUserById([FromRoute] string id)
        {
            
            Guid userId = Guid.Empty;
            User user = null;
            
            try
            {
                userId = Guid.Parse(id);
            }
            catch
            {
                return BadRequest("User id not valid");
            }
            
            user = _repo.GetById(userId);
            if (user == null)
            {
                return NotFound("User not existing");
            }
            return Ok(user);
        }
        
        [HttpGet("{id}/books")]
        public ActionResult GetUserBooks([FromRoute] string id)
        {
            Guid userId = Guid.Empty;
            User user = null;
            
            try
            {
                userId = Guid.Parse(id);
            }
            catch
            {
                return BadRequest("User id not valid");
            }
            
            user = _repo.GetById(userId);
            if (user == null)
            {
                return NotFound("User not existing");
            }
            
            var u = _repo.GetById(userId);
            return Ok(u.Books);
        }
        
//        [HttpPost("{id}/books/add")]
//        public ActionResult AddBook([FromBody] BookDto book)
//        {
//            return Ok();
//        }
    }
}