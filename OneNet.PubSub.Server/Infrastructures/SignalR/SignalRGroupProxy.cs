using System.Threading.Tasks;
using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    /// <summary>
    /// To manage set of connection in one group.
    /// Keep feature signalR group and tracking connection.
    /// </summary>
    public class SignalRGroupProxy
    {
        private BaseHub _hub;
        private string _groupName;
        private readonly ConnectionList _connectionList;

        public SignalRGroupProxy()
        {
            _connectionList = new ConnectionList();
        }


        public async Task AddClientToGroup(Connection connection)
        {
            await _hub.Groups.AddToGroupAsync(connection.Id, _groupName);
            _connectionList.Add(connection);
        }

        public async Task RemoveClientFromGroup(string connectionId)
        {
            await _hub.Groups.RemoveFromGroupAsync(connectionId, _groupName);
            _connectionList.Remove(connectionId);
        }

        private async Task RemoveClientFromGroup(Connection connection)
        {
            await _hub.Groups.RemoveFromGroupAsync(connection.Id, _groupName);
            _connectionList.Remove(connection.Id);
        }


        public async Task RemoveAllClient()
        {
            var data = _connectionList.GetConnections();
            foreach (var connection in data)
            {
                await _hub.Groups.RemoveFromGroupAsync(connection.Id, _groupName);
                _connectionList.Remove(connection.Id);
            }
        }

        public SignalRGroupProxy UpdateHub(BaseHub hub)
        {
            _hub = hub;
            _groupName = hub.Name;
            return this;
        }
    }
}