namespace OneNet.PubSub.Server.Application.Domains
{
    public class TopicConfig
    {
        public bool IsKeepTopicWhenOwnerDisconnect { get; set; }
        public bool IsUpdateOwnerConnection { get; set; }

        public static TopicConfig CreateDefault()
        {
            return new TopicConfig()
            {
                IsKeepTopicWhenOwnerDisconnect = true,
                IsUpdateOwnerConnection = true,
            };
        }
    }
}