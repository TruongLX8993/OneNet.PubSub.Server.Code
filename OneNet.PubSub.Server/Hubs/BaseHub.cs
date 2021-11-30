using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Infrastructures.SignalR;

namespace OneNet.PubSub.Server.Hubs
{
    public class BaseHub : Hub
    {
        private string _name;
        private readonly HubConnectionManager _hubConnectionManager;
        private readonly GroupManager _groupManager;
        
        protected BaseHub()
        {
            _hubConnectionManager = HubConnectionManagerPool.Instance.Get(this);
            _groupManager = GroupManagerFactory.Instance.Get(this);
        }
        
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                    return _name;

                if (GetType()
                    .GetCustomAttributes(typeof(HubNameAttr), true)
                    .FirstOrDefault() is HubNameAttr hubNameAttr) return _name = hubNameAttr.Name;
                return "";
            }
        }

        public GroupProxy GetGroupProxy(string groupName) => _groupManager.GetGroupProxy(groupName);
        protected HubConnectionManager GetHubConnectionManager() => _hubConnectionManager;
        protected int GetNumberConnections() => _hubConnectionManager.GetNumberConnection();

        public async Task RemoveAllClientFromGroup(string groupName)
        {
            await GetGroupProxy(groupName).RemoveAllClient();
        }

        public Connection GetCurrentConnection()
        {
            var connectionId = Context.ConnectionId;
            return _hubConnectionManager.GetById(connectionId);
        }
    }
}