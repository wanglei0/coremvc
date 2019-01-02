using Microsoft.AspNetCore.Mvc;
using WebApp.Routing;

namespace WebApp.Controllers
{
    [Route("api/method")]
    [ApiController]
    public class MethodRoutingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return new ActionResult<string>("GET");
        }
        
        [HttpEat]
        public ActionResult<string> Eat()
        {
            return new ActionResult<string>("EAT");
        }

        [HttpGet, HttpPost, HttpDelete]
        [Route("multiple")]
        public ActionResult<string> MapToMultipleMethods()
        {
            var method = Request.Method.ToUpperInvariant();
            return new ActionResult<string>($"Multiple:{method}");
        }
    }
}
