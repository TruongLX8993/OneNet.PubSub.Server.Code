using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneNet.PubSub.Server.Hubs.Models
{
    public class SignalRGroupProxy
    {
        private BaseHub _hub;

        private static readonly IDictionary<string, ConnectionList> _connectionsInGroup =
            new ConcurrentDictionary<string, ConnectionList>();

        public void UpdateHub(BaseHub hub)
        {
            _hub = hub;
        }

        public async Task AddClientToGroup(string groupName, Connection connection)
        {
            await _hub.Groups.AddToGroupAsync(connection.Id, groupName);
            var connectionList = GetOrCreateConnectionList(groupName);
            connectionList.Add(connection);
        }

        public async Task RemoveClientFromGroup(string groupName, string connectionId)
        {
            await _hub.Groups.RemoveFromGroupAsync(connectionId, groupName);
            var connectionList = GetConnectionList(groupName);
            connectionList?.Remove(connectionId);
        }

        public async Task RemoveClientFromGroup(string groupName, Connection connection)
        {
            await _hub.Groups.RemoveFromGroupAsync(connection.Id, groupName);
            var connectionList = GetConnectionList(groupName);
            connectionList?.Remove(connection.Id);
        }


        private ConnectionList GetConnectionList(string groupName)
        {
            return _connectionsInGroup.ContainsKey(groupName) ? _connectionsInGroup[groupName] : null;
        }

        private ConnectionList GetOrCreateConnectionList(string groupName)
        {
            if (!_connectionsInGroup.ContainsKey(groupName))
            {
                _connectionsInGroup.Add(groupName, new ConnectionList());
            }

            return _connectionsInGroup[groupName];
        }

        public async Task RemoveAllClient(string groupName)
        {
            var connectionList = GetConnectionList(groupName);
            if (connectionList == null)
                return;
            var data = connectionList.GetConnections();
            foreach (var connectionId in data)
                await RemoveClientFromGroup(groupName, connectionId);
        }
    }
}