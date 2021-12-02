using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Infrastructures.SignalR;

namespace OneNet.PubSub.Server.Hubs
{
    public class BaseHub : Hub
    {
        protected BaseHub()
        {
        }

        public GroupProxy GetGroupProxy(string groupName) => GroupManagerFactory.Instance.Get(this)
            .GetGroupProxy(groupName);

        protected ConnectionManager ConnectionManager => HubConnectionManagerPool.Instance.Get(this);

        protected int GetNumberConnections()
        {
            return ConnectionManager.GetNumberConnection();
        }

        public async Task RemoveAllClientFromGroup(string groupName)
        {
            await GetGroupProxy(groupName)
                .RemoveAllClient();
        }

        public Connection GetCurrentConnection()
        {
            var connectionId = Context.ConnectionId;
            var res = ConnectionManager.GetById(connectionId);
            if (res == null)
                throw new Exception($"Not found current connection with id:{connectionId}");
            return res;
        }

        public static string GetName<T>() where T : BaseHub
        {
            var type = typeof(T);
            return GetName(type);
        }

        private static string GetName(Type type)
        {
            var hubNameAttr = type.GetCustomAttributes(typeof(HubNameAttr), true)
                .FirstOrDefault() as HubNameAttr;
            return hubNameAttr?.Name;
        }

        // public string Name => GetName(GetType());
    }
}