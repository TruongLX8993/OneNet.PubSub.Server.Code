using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.Interfaces
{
    public interface ISubscription
    {
        Task Subscribe(Topic topic,Connection connection);
        Task UnSubscribe(Topic topic, Connection connection);
    }
}