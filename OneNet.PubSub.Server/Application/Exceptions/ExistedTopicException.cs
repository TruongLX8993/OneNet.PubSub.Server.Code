namespace OneNet.PubSub.Server.Application.Exceptions
{
    public class ExistedTopicException : ApplicationException
    {
        public ExistedTopicException(string topicName) : base($"existed_topic: {topicName}")
        {
        }
    }
}