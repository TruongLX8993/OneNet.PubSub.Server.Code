namespace OneNet.PubSub.Server.Application.DTOs
{
    public class CreateTopicResponse
    {
        public TopicDTO TopicDto { get; }
        public bool IsNewTopic { get; }

        public CreateTopicResponse(TopicDTO topicDto, bool isNewTopic = true)
        {
            TopicDto = topicDto;
            IsNewTopic = isNewTopic;
        }
    }
}