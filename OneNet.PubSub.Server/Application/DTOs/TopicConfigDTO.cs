namespace OneNet.PubSub.Server.Application.DTOs
{
    public class TopicConfigDTO
    {
        public bool IsKeepTopicWhenOwnerDisconnect { get; set; }
        public bool IsUpdateOwnerConnection { get; set; }

        public static TopicConfigDTO CreateDefault()
        {
            return new TopicConfigDTO()
            {
                IsKeepTopicWhenOwnerDisconnect = true,
                IsUpdateOwnerConnection = true,
            };
        }
    }
}