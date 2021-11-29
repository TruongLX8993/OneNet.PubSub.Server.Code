using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.Interfaces
{
    public interface INotification
    {
        Task OnAbortTopic(Topic topic);
        Task OnCreateTopic(Topic topic);
    }
}