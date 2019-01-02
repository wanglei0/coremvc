using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/binding")]
    [ApiController]
    public class BindingTestController : ControllerBase
    {
        [HttpGet("hardcoded")]
        public ActionResult<string> GetHardCoded()
        {
            return new ActionResult<string>("hardcoded");
        }

        [HttpGet("hardcoded-query")]
        public ActionResult<string> GetHardCodedQuery(string message)
        {
            return new ActionResult<string>(message.ToUpper());
        }
        
        [HttpGet("hardcoded-query-integer")]
        public ActionResult<string> GetHardCodedQueryInteger(int value)
        {
            return new ActionResult<string>(value.ToString("G"));
        }
        
        [HttpGet("hardcoded-required-query-integer")]
        public ActionResult<string> GetHardCodedRequiredQueryParameter(
            [FromQuery, Required]int value)
        {
            return new ActionResult<string>(value.ToString("G"));
        }
    }
}