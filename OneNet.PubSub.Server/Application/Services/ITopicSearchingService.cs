using System.Collections.Generic;
using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.DTOs;

namespace OneNet.PubSub.Server.Application.Services
{
    public interface ITopicSearchingService
    {
        Task<TopicDTO> GetByName(string name);
        Task<IList<TopicDTO>> Search(SearchTopicRequest searchTopicRequest);
    }
}