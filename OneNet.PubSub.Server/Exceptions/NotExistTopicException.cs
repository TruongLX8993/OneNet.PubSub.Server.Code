using Microsoft.AspNetCore.SignalR;

namespace OneNet.PubSub.Server.Exceptions
{
    public class NotExistTopicException : HubException
    { 
        public NotExistTopicException() : base("not_exist_topic")
        {
            
        }
    }
}