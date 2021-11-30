using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    /// <summary>
    /// To manage all connection in hub.
    /// </summary>
    public class ConnectionManager
    {
        private readonly ConnectionList _connectionList;
        private string _hubName;

        public ConnectionManager()
        {
            _connectionList = new ConnectionList();
            _hubName = nameof(_hubName);
        }

        public void AddConnection(string connectionId, string username)
        {
            var connection = new Connection()
            {
                Id = connectionId,
                HubName = _hubName,
                UserName = username
            };
            _connectionList.Add(connection);
        }

        public void RemoveConnection(string connectionId)
        {
            _connectionList.Remove(connectionId);
        }

        public int GetNumberConnection()
        {
            return _connectionList.Count;
        }

        public Connection GetById(string id)
        {
            return _connectionList.GetById(id);
        }
    }
}