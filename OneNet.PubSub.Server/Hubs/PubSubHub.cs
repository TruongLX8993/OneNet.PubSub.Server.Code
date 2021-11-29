using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Repository;
using OneNet.PubSub.Server.Application.Services;
using OneNet.PubSub.Server.Infrastructures.SignalR;

namespace OneNet.PubSub.Server.Hubs
{
    [HubNameAttr("pub-sub")]
    public class PubSubHub : BaseHub
    {
        private readonly ILogger<PubSubHub> _logger;
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicService _topicService;


        public PubSubHub(
            ILogger<PubSubHub> logger,
            ITopicRepository topicRepository)
        {
            _logger = logger;
            _topicRepository = topicRepository;
            _topicService = new TopicService(topicRepository,
                new CurrentConnection(this),
                new Notification(this),
                new MessageSender(this),
                new Subscription(this));
        }

        public override Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()
                .Request.Query["username"];
            GetHubConnectionManager()
                .AddConnection(Context.ConnectionId, username);
            _logger.LogInformation(
                $"{nameof(OnConnectedAsync)}-NumberConnection:{GetHubConnectionManager().GetNumberConnection()}");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var topics = await _topicRepository.GetByOwnerConnectionId(Context.ConnectionId);
            var canAbortTopic = topics.Where(topic => topic.IsAbortWhenOwnerDisconnect())
                .ToList();
            foreach (var topic in canAbortTopic)
            {
                await AbortTopic(topic);
            }


            GetHubConnectionManager()
                .RemoveConnection(Context.ConnectionId);
            _logger.LogInformation(
                $"{nameof(OnDisconnectedAsync)}-NumberConnection:{GetHubConnectionManager().GetNumberConnection()}");
            await base.OnDisconnectedAsync(exception);
        }


        [HubMethodName("create-topic")]
        public async Task CreateTopic(string topicName, TopicConfigDTO topicConfig)
        {
            topicConfig ??= TopicConfigDTO.CreateDefault();
            _logger.LogInformation($"{nameof(CreateTopic)}:{topicName}");
            _logger.LogInformation($"{nameof(CreateTopic)}:{JsonConvert.SerializeObject(topicConfig)}");

            var res = await _topicService.CreateTopic(topicName, topicConfig);
            if (res.IsNewTopic)
                await Clients.All.SendAsync("onNewTopic", res.TopicDto);
        }

        [HubMethodName("subscribe")]
        public async Task Subscribe(string topicName)
        {
            await _topicService.Subscribe(topicName);
        }

        [HubMethodName("un-subscribe")]
        public async Task UnSubscribe(string topicName)
        {
            await _topicService.UnSubscribe(topicName);
            await GetGroupProxy(topicName)
                .RemoveClientFromGroup(Context.ConnectionId);
        }

        [HubMethodName("publish")]
        public async Task Publish(string topic, object data)
        {
            await _topicService.SendMessage(topic, data);
        }

        private async Task AbortTopic(Topic topic)
        {
            await _topicService.AbortTopic(topic.Name);
        }
    }
}