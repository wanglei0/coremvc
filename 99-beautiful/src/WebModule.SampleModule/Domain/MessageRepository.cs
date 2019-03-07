using System.Collections.Generic;

namespace WebModule.SampleModule.Domain
{
    class MessageRepository
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