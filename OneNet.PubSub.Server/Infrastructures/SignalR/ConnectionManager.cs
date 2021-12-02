using System.Diagnostics;
using Newtonsoft.Json;
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
        private readonly object _lock = new object();
        public ConnectionManager()
        {
            _connectionList = new ConnectionList();
            _hubName = nameof(_hubName);
        }

        public void AddConnection(string connectionId, string username)
        {
            lock (_lock)
            {
                var connection = new Connection()
                {
                    Id = connectionId,
                    HubName = _hubName,
                    UserName = username
                };
                _connectionList.Add(connection);
                Debug.WriteLine($"{nameof(AddConnection)}: {connectionId}, {username},{GetHashCode()}");
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (_lock)
            {
                _connectionList.Remove(connectionId);
            }
        }

        public int GetNumberConnection()
        {
            lock (_lock)
            {
                return _connectionList.Count;
            }
        }

        public Connection GetById(string id)
        {
            lock (_lock)
            {
                var res = _connectionList.GetById(id);
                if (res == null)
                    Debug.WriteLine($"{nameof(GetById)}: {id},{GetHashCode()}");
                return res;
            }
        }
    }
}