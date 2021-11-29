using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Repository;

namespace OneNet.PubSub.Server.Infrastructures.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IList<Topic> _topics = new List<Topic>();

        public Task<IList<Topic>> Search(string name)
        {
            var res = _topics;
            if (!string.IsNullOrEmpty(name))
            {
                res = _topics.Where(tp => tp.Name.Contains(name))
                    .ToList() as IList<Topic>;
            }

            return Task.FromResult(res);
        }

        public Task<Topic> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Topic's Name not empty");

            var res = _topics
                .FirstOrDefault(tp => tp.Name == name);
            return Task.FromResult(res);
        }

        public Task Add(Topic topic)
        {
            _topics.Add(topic);
            return Task.CompletedTask;
        }

        public Task<bool> ExistTopic(string name)
        {
            var topic = _topics
                .FirstOrDefault(tp => tp.Name == name);
            return Task.FromResult(topic != null);
        }

        public Task<List<Topic>> GetByOwnerConnectionId(string contextConnectionId)
        {
            return Task.FromResult(_topics.Where(tp => tp.OwnerConnectionId == contextConnectionId)
                .ToList());
        }

        public Task Remove(Topic topic)
        {
            _topics.Remove(topic);
            return Task.CompletedTask;
        }
    }
}