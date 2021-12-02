using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Impls
{
    public class MessageSender : IMessageSender
    {
        private readonly IHubContext<PubSubHub> _hubContext;

        public MessageSender(IHubContext<PubSubHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessage(Topic topic, object data)
        {
            await _hubContext.Clients.Group(topic.Name)
                .SendAsync("onNewMessage", topic.Name, data);
        }
    }
}