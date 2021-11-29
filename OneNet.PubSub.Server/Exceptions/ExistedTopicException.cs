using Microsoft.AspNetCore.SignalR;

namespace OneNet.PubSub.Server.Exceptions
{
    public class ExistedTopicException : HubException
    {
        public ExistedTopicException() : base("existed_topic")
        {
            
        }
    }
}