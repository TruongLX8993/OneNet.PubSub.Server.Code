using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Exceptions;
using OneNet.PubSub.Server.Application.Repository;

namespace OneNet.PubSub.Server.Application.Services
{
    public class TopicSearchingService : ITopicSearchingService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicSearchingService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<IList<TopicDTO>> Search(SearchTopicRequest searchTopicRequest)
        {
            var topics = await _topicRepository.Search(searchTopicRequest.Name);
            var rs = topics.Select(tp => new TopicDTO(tp))
                .ToList();
            return rs;
        }

        public async Task<TopicDTO> GetByName(string name)
        {
            var topic = await _topicRepository.GetByName(name);
            if (topic == null)
                throw new NotFoundTopicException(name);
            return new TopicDTO(topic);
        }
    }
}