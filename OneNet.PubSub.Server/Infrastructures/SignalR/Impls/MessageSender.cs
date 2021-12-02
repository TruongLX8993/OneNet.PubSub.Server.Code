using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Impls
{
    public class MessageSender : IMessageSender
    {
        private readonly BaseHub _baseHub;

        public MessageSender(BaseHub baseHub)
        {
            _baseHub = baseHub;
        }

        public async Task SendMessage(Topic topic, object data)
        {
            await _baseHub.Clients.Group(topic.Name)
                .SendAsync("onNewMessage", topic.Name, data);
        }
    }
}