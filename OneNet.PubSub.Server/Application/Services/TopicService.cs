using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Application.Repository;
using OneNet.PubSub.Server.Exceptions;

namespace OneNet.PubSub.Server.Application.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ICurrentConnection _currentConnection;
        private readonly INotification _notification;

        private readonly IMessageSender _messageSender;
        private readonly ISubscription _subscription;


        public TopicService(
            ITopicRepository topicRepository,
            ICurrentConnection currentConnection,
            INotification notification,
            IMessageSender messageSender, 
            ISubscription subscription)
        {
            _topicRepository = topicRepository;
            _currentConnection = currentConnection;
            _notification = notification;
            _messageSender = messageSender;
            _subscription = subscription;
        }

        public async Task<CreateTopicResponse> CreateTopic(
            string topicName,
            TopicConfigDTO topicConfig)
        {
            var topic = await _topicRepository.GetByName(topicName);
            var currentConnection = _currentConnection.GetConnection();

            if (topic != null)
            {
                var topicDTO = new TopicDTO(topic);
                if (topic.IsOwnerConnection(currentConnection))
                    return new CreateTopicResponse(topicDTO, false);

                if (!topic.CanUpdateOwnerConnection(currentConnection)) throw new ExistedTopicException();

                topic.UpdateConnectionOwner(currentConnection, true);
                return new CreateTopicResponse(topicDTO, false);
            }

            topic = new Topic()
            {
                Name = topicName,
                OwnerConnection = currentConnection,
                TopicConfig = new TopicConfig()
                {
                    IsKeepTopicWhenOwnerDisconnect = topicConfig.IsKeepTopicWhenOwnerDisconnect,
                    IsUpdateOwnerConnection = topicConfig.IsUpdateOwnerConnection
                }
            };

            await _topicRepository.Add(topic);
            await _notification.OnCreateTopic(topic);
            var res = new CreateTopicResponse(new TopicDTO(topic));
            return res;
        }

        public async Task AbortTopic(string topicName)
        {
            var topic = await _topicRepository.GetByName(topicName);
            await _topicRepository.Remove(topic);
            await _notification.OnAbortTopic(topic);
        }

        public async Task SendMessage(string topicName, object data)
        {
            var topic = await _topicRepository.GetByName(topicName);
            if (topic == null)
                throw new NotExistTopicException();
            await _messageSender.SendMessage(topic, data);
        }

        public async Task UnSubscribe(string topicName)
        {
            var topic = await _topicRepository.GetByName(topicName);
            if (topic == null)
                throw new NotExistTopicException();
            var currentConnection = _currentConnection.GetConnection();
            await _subscription.UnSubscribe(topic, currentConnection);
        }

        public async Task Subscribe(string topicName)
        {
            var topic = await _topicRepository.GetByName(topicName);
            if (topic == null)
                throw new NotExistTopicException();
            var currentConnection = _currentConnection.GetConnection();
            await _subscription.Subscribe(topic, currentConnection);
        }
    }
}