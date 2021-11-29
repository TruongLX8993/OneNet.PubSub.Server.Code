using System.Linq;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Infrastructures.SignalR;

namespace OneNet.PubSub.Server.Hubs
{
    public class BaseHub : Hub
    {
        private string _name;

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

        public SignalRGroupProxy GetGroupProxy(string groupName)
        {
            return SignalRGroupManagerFactory.Instance.Get(this)
                .GetGroupProxy(groupName);
        }

        public HubConnectionManager GetHubConnectionManager()
        {
            return HubConnectionManagerPool.Instance.Get(this);
        }
    }
}