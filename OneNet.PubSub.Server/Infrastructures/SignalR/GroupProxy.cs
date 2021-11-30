using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    /// <summary>
    /// To manage set of connection in one group.
    /// Keep feature signalR group and tracking connection.
    /// </summary>
    public class GroupProxy
    {
        private IGroupManager _groupManager;
        private readonly string _groupName;
        private readonly ConnectionList _connectionList;

        public GroupProxy(string groupName)
        {
            _groupName = groupName;
            _connectionList = new ConnectionList();
        }


        public async Task AddClientToGroup(Connection connection)
        {
            await _groupManager.AddToGroupAsync(connection.Id, _groupName);
            _connectionList.Add(connection);
        }

        public async Task RemoveClientFromGroup(string connectionId)
        {
            await _groupManager.RemoveFromGroupAsync(connectionId, _groupName);
            _connectionList.Remove(connectionId);
        }

        private async Task RemoveClientFromGroup(Connection connection)
        {
            await _groupManager.RemoveFromGroupAsync(connection.Id, _groupName);
            _connectionList.Remove(connection.Id);
        }


        public async Task RemoveAllClient()
        {
            var data = _connectionList.GetConnections();
            foreach (var connection in data)
            {
                await _groupManager.RemoveFromGroupAsync(connection.Id, _groupName);
                _connectionList.Remove(connection.Id);
            }
        }

        public GroupProxy UpdateGroupManager(IGroupManager groupManager)
        {
            _groupManager = groupManager;
            return this;
        }
    }
}