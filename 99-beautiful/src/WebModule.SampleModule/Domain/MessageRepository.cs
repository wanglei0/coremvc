using System.Collections.Generic;

namespace WebModule.SampleModule.Domain
{
    public class MessageRepository
    {
        public IEnumerable<Message> GetAllMessages()
        {
            return new[]
            {
                new Message("Hello"),
                new Message("World")
            };
        }
    }
}