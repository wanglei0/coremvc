using Microsoft.AspNetCore.Mvc;
using WebModule.SampleModule.Domain;

namespace WebModule.SampleModule.Controllers
{
    [ApiController]
    [Route("message")]
    class MessageController : Controller
    {
        readonly MessageRepository messageRepository;

        public MessageController(MessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return new JsonResult(messageRepository.GetAllMessages());
        }
    }
}