using Microsoft.AspNetCore.SignalR;

namespace OneNet.PubSub.Server.Exceptions
{
    public class NotExistTopicException : HubException
    {
        public NotExistTopicException(string topicName) : base($"not_exist_topic:{topicName}")
        {
            
        }
    }
}