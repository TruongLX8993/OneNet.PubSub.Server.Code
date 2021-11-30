using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.DTOs;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class Notification : INotification
    {
        private readonly BaseHub _hub;

        public Notification(BaseHub hub)
        {
            _hub = hub;
        }
        public async Task OnAbortTopic(Topic topic)
        {
            await _hub.Clients.Group(topic.Name)
                .SendAsync("onAbortTopic", new TopicDTO(topic));
            await _hub.RemoveAllClientFromGroup(topic.Name);
        }

        public async Task OnCreateTopic(Topic topic)
        {
            await _hub.Clients.All.SendAsync("onNewTopic", new TopicDTO(topic));
        }
    }
}