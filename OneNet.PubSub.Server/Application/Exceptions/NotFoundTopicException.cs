namespace OneNet.PubSub.Server.Application.Exceptions
{
    public class NotFoundTopicException : ApplicationException
    {
        public NotFoundTopicException(string topicName) : base($"not_exist_topic: {topicName}")
        {
        }
    }
}