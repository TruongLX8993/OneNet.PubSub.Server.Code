using System.Collections.Generic;

namespace OneNet.PubSub.Server.Models
{
    public class TopicManager
    {
        private string _hubName;

        public TopicManager(string hubName)
        {
            _hubName = hubName;
        }

        public void Add(Topic topic)
        {
        }

        public void Remove(Topic topic)
        {
        }

        public IReadOnlyList<string> GetAll()
        {
            return null;
        }
    }
}