using Microsoft.AspNetCore.Mvc;
using WebApp.Routing;

namespace WebApp.Controllers
{
    public class CentralizedMethodRoutingController : ControllerBase
    {
        public ActionResult<string> Get()
        {
            return new ActionResult<string>("GET");
        }
        
        public ActionResult<string> Eat()
        {
            return new ActionResult<string>("EAT");
        }

        public ActionResult<string> MapToMultipleMethods()
        {
            var method = Request.Method.ToUpperInvariant();
            return new ActionResult<string>($"Multiple:{method}");
        }
    }
}