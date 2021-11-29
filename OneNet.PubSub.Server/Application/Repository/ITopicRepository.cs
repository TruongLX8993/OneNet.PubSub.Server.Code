using System.Collections.Generic;
using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.Repository
{
    public interface ITopicRepository
    {
        Task<IList<Topic>> Search(string name);
        Task<Topic> GetByName(string name);
        Task Add(Topic topic);
        Task<bool> ExistTopic(string name);
        Task<List<Topic>> GetByOwnerConnectionId(string contextConnectionId);
        Task Remove(Topic topic);
    }
}