namespace OneNet.PubSub.Server.Hubs.Models
{
    public class ConnectionManager
    {
        private readonly ConnectionList _connectionList;
        private readonly string _hubName;

        public ConnectionManager(string hubName)
        {
            _connectionList = new ConnectionList();
            _hubName = hubName;
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