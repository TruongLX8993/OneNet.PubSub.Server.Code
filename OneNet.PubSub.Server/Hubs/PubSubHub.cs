#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneNet.PubSub.Server.DTOs;
using OneNet.PubSub.Server.Exceptions;
using OneNet.PubSub.Server.Hubs.Models;
using OneNet.PubSub.Server.Models;
using OneNet.PubSub.Server.Repository;

namespace OneNet.PubSub.Server.Hubs
{
    public class PubSubHub : BaseHub
    {
        private readonly ILogger<PubSubHub> _logger;
        private readonly ITopicRepository _topicRepository;
        private static readonly HubConnectionManager HubConnectionManager = new HubConnectionManager(nameof(PubSubHub));
        private readonly SignalRGroupProxy _signalRGroupProxy;

        public PubSubHub(
            ILogger<PubSubHub> logger,
            ITopicRepository topicRepository) : base("pub-sub")
        {
            _logger = logger;
            _topicRepository = topicRepository;
            _signalRGroupProxy = SignalRGroupProxyManagerFactory.Instance.GetGroupProxy(this);
        }

        public override Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()
                .Request.Query["username"];
            HubConnectionManager.AddConnection(Context.ConnectionId, username);
            _logger.LogInformation(
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                $"{nameof(OnConnectedAsync)}-NumberConnection:{HubConnectionManager.GetNumberConnection()}");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var topics = await _topicRepository.GetByOwnerConnectionId(Context.ConnectionId);
            var canAbortTopic = topics.Where(topic => topic.IsAbortWhenOwnerDisconnect())
                .ToList();
            foreach (var topic in canAbortTopic)
            {
                await AbortTopic(topic);
            }


            HubConnectionManager.RemoveConnection(Context.ConnectionId);
            _logger.LogInformation(
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                $"{nameof(OnDisconnectedAsync)}-NumberConnection:{HubConnectionManager.GetNumberConnection()}");
            base.OnDisconnectedAsync(exception);
        }


        [HubMethodName("create-topic")]
        public async Task CreateTopic(string topicName, TopicConfig topicConfig)
        {
            topicConfig ??= TopicConfig.CreateDefault();
            _logger.LogInformation($"{nameof(CreateTopic)}:{topicName}");
            _logger.LogInformation($"{nameof(CreateTopic)}:{JsonConvert.SerializeObject(topicConfig)}");
            var topic = await _topicRepository.GetByName(topicName);
            var currentConnection = GetCurrentConnection();

            if (topic != null)
            {
                if (topic.IsOwnerConnection(currentConnection))
                    return;

                if (!topic.CanUpdateOwnerConnection(currentConnection)) throw new ExistedTopicException();

                topic.UpdateConnectionOwner(currentConnection, true);
                return;
            }

            topic = new Topic()
            {
                Name = topicName,
                OwnerConnection = currentConnection,
                TopicConfig = topicConfig
            };

            await _topicRepository.Add(topic);
            await Clients.All.SendAsync("onNewTopic", new TopicDTO(topic));
        }

        [HubMethodName("subscribe")]
        public async Task Subscribe(string topicName)
        {
            var topic = await _topicRepository.GetByName(topicName);
            if (topic == null)
                throw new NotExistTopicException();
            await _signalRGroupProxy.AddClientToGroup(topicName, HubConnectionManager.GetById(Context.ConnectionId));
        }

        [HubMethodName("un-subscribe")]
        public async Task UnSubscribe(string topicName)
        {
            var topic = await _topicRepository.GetByName(topicName);
            if (topic != null)
            {
                if (topic.OwnerConnectionId == Context.ConnectionId)
                    await AbortTopic(topic);
            }


            await _signalRGroupProxy.RemoveClientFromGroup(topicName, Context.ConnectionId);
        }

        [HubMethodName("publish")]
        public async Task Publish(string topic, object data)
        {
            await Clients.Group(topic)
                .SendAsync("onNewMessage",
                    topic,
                    data);
        }

        private async Task AbortTopic(Topic topic)
        {
            _logger.LogInformation($"{nameof(AbortTopic)}-{topic}");
            await Clients.Group(topic.Name)
                .SendAsync("onAbortTopic", new TopicDTO(topic));
            await _signalRGroupProxy.RemoveAllClient(topic.Name);
            await _topicRepository.Remove(topic);
        }

        private Connection GetCurrentConnection()
        {
            return HubConnectionManager.GetById(Context.ConnectionId);
        }
    }
}