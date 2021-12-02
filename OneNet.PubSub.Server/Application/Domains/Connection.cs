using System.Collections.Generic;

namespace OneNet.PubSub.Server.Application.Domains
{
    public class Connection
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string HubName { get; set; }
        public IList<string> SubscribedTopics { get; } = new List<string>();

        public void AddTopic(string topic)
        {
            if (!SubscribedTopics.Contains(topic))
                SubscribedTopics.Add(topic);
        }

        public void RemoveAllTopics()
        {
            SubscribedTopics.Clear();
        }
    }
}