using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Impls
{
    public class Subscription : ISubscription
    {
        private readonly IHubContext<PubSubHub> _hubContext;

        public Subscription(IHubContext<PubSubHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task Subscribe(Topic topic, Connection connection)
        {
            var groupManagerProxy = GroupManagerFactory.Instance.Get(_hubContext);
            await groupManagerProxy.GetGroupProxy(topic.Name)
                .AddClientToGroup(connection);
        }

        public async Task UnSubscribe(Topic topic, Connection connection)
        {
            var group = GetGroup(topic.Name);
            await group.RemoveClientFromGroup(connection.Id);
        }

        public async Task UnSubscribe(Connection currentConnection)
        {
            var groupNames = currentConnection.SubscribedTopics;
            foreach (var groupName in groupNames)
            {
                await GetGroup(groupName)
                    .RemoveClientFromGroup(currentConnection.Id);
            }
        }

        private GroupProxy GetGroup(string name)
        {
            var groupManagerProxy = GroupManagerFactory.Instance.Get(_hubContext);
            return groupManagerProxy.GetGroupProxy(name);
        }
    }
}