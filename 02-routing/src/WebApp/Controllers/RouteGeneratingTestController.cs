using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api/route-generating")]
    public class RouteGeneratingTestController : ControllerBase
    {
        [HttpGet("users/{userId}", Order = 9, Name = "get_user_by_id")]
        public ActionResult<string> GetUserName(string userId)
        {
            return new ActionResult<string>($"user: {userId}");
        }

        [HttpGet("users/api", Order = 0)]
        public ActionResult<string> GetUserNameRoute()
        {
            return Url.Action("GetUserName", new {userId = "3"});
        }

        [HttpGet("users/api/{routeName}", Order = 1)]
        public ActionResult<string> GetUserNameRouteByName(string routeName)
        {
            return Url.RouteUrl(routeName, new {userId = "3"}); 
        }
    }
}