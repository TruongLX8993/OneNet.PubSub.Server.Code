using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class Subscription : ISubscription
    {
        private readonly BaseHub _hub;

        public Subscription(BaseHub hub)
        {
            _hub = hub;
        }

        public async Task Subscribe(Topic topic, Connection connection)
        {
            await _hub.GetGroupProxy(topic.Name)
                .AddClientToGroup(connection);
        }

        public async Task UnSubscribe(Topic topic, Connection connection)
        {
            await _hub.GetGroupProxy(topic.Name)
                .RemoveClientFromGroup(connection.Id);
        }

        public async Task UnSubscribe(Connection currentConnection)
        {
            var groupNames = currentConnection.SubscribedTopics;
            foreach (var groupName in groupNames)
            {
                await _hub.GetGroupProxy(groupName)
                    .RemoveClientFromGroup(currentConnection.Id);
            }
        }
    }
}