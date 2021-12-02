using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Impls
{
    public class Notification : INotification
    {
        private readonly IHubContext<PubSubHub> _hub;

        public Notification(IHubContext<PubSubHub> hub)
        {
            _hub = hub;
        }
        public async Task OnAbortTopic(Topic topic)
        {
            await _hub.Clients.Group(topic.Name)
                .SendAsync("onAbortTopic", new TopicDTO(topic));
        }

        public async Task OnCreateTopic(Topic topic)
        {
            await _hub.Clients.All.SendAsync("onNewTopic", new TopicDTO(topic));
        }
    }
}