using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.DTOs;

namespace OneNet.PubSub.Server.Application.Services
{
    public interface ITopicService
    {
        Task<CreateTopicResponse> CreateTopic(string topicName, TopicConfigDTO topicConfig);
        Task AbortTopic(string topicName);
        Task SendMessage(string topicName, object data);
        Task UnSubscribe(string topicName);
        Task Subscribe(string topicName);
    }
}