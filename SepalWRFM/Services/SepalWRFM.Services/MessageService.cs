using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
