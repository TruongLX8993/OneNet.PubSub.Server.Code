using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Application.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessage(Topic topic, object data);
    }
}