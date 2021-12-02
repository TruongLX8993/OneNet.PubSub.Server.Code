using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessage(Topic topic, object data);
    }
}