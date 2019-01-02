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

        [HttpGet("hardcoded-validated-query-integer")]
        public ActionResult<string> GetHardCodedValidatedQueryParameter(
            [FromQuery, MaxLength(10)] string text)
        {
            return new ActionResult<string>(text);
        }

        [HttpGet("path/user/{userId}")]
        public ActionResult<string> GetUserIdFromSegment(int userId)
        {
            return new ActionResult<string>(userId.ToString("D"));
        }
        
        [HttpGet("path/email/{email}")]
        public ActionResult<string> GetUserIdFromSegment(string email)
        {
            return new ActionResult<string>(email);
        }
        
        [HttpGet("path/optional/{optional=default}")]
        public ActionResult<string> GetOptionalVariableWithDefaultValue(string optional)
        {
            return new ActionResult<string>(optional);
        }
        
        [HttpGet("path/optional-no-default/{optional?}")]
        public ActionResult<string> GetOptionalVariableWithoutDefaultValue(string optional)
        {
            return new ActionResult<string>(optional ?? "(null value)");
        }
    }
}