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
                new CurrentConnectionService(this),
                new Notification(this),
                new MessageSender(this),
                new Subscription(this));
        }

        public override Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()
                .Request.Query["username"];
            ConnectionManager
                .AddConnection(Context.ConnectionId, username);
            _logger.LogInformation(
                $"{nameof(OnConnectedAsync)}-NumberConnection:{ConnectionManager.GetNumberConnection()}");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
      

            await _topicService.UnSubscribeAll();
            foreach (var topic in canAbortTopic)
                await AbortTopic(topic);

            ConnectionManager.RemoveConnection(Context.ConnectionId);
            _logger.LogInformation(
                $"{nameof(OnDisconnectedAsync)}-NumberConnection:{ConnectionManager.GetNumberConnection()}");
            await base.OnDisconnectedAsync(exception);
        }


        [HubMethodName("create-topic")]
        public async Task CreateTopic(string topicName, TopicConfigDTO topicConfig)
        {
            topicConfig ??= TopicConfigDTO.CreateDefault();
            var res = await _topicService.CreateTopic(topicName, topicConfig);
            _logger.LogInformation($"{nameof(CreateTopic)} is successfully: {JsonConvert.SerializeObject(res)}");
        }


        [HubMethodName("un-subscribe")]
        public async Task UnSubscribe(string topicName)
        {
            await _topicService.UnSubscribe(topicName);
            await GetGroupProxy(topicName)
                .RemoveClientFromGroup(Context.ConnectionId);
            _logger.LogInformation($"{nameof(UnSubscribe)}-{Context.ConnectionId}:{topicName}");
        }

        [HubMethodName("subscribe")]
        public async Task Subscribe(string topicName)
        {
            await _topicService.Subscribe(topicName);
            _logger.LogInformation($"{nameof(Subscribe)}-{Context.ConnectionId}:{topicName}");
            // await Groups.AddToGroupAsync(Context.ConnectionId, topicName);
        }

        [HubMethodName("publish")]
        public async Task Publish(string topic, object data)
        {
            await _topicService.SendMessage(topic, data);
            _logger.LogInformation(
                $"{nameof(Publish)}-{Context.ConnectionId}:{topic}-{JsonConvert.SerializeObject(data)}");
            // await Clients.Group(topic)
            //     .SendAsync("onNewMessage", topic, data);
        }

        private async Task AbortTopic(Topic topic)
        {
            await _topicService.AbortTopic(topic.Name);
        }
    }
}