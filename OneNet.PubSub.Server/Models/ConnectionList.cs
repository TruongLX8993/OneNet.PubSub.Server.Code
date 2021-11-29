using System.Collections.Generic;
using System.Linq;

namespace OneNet.PubSub.Server.Models
{
    public class ConnectionList
    {
        private readonly IList<Connection> _connections;

        public ConnectionList()
        {
            _connections = new List<Connection>();
        }

        public int Count => _connections.Count;

        public void Add(Connection connection)
        {
            _connections.Add(connection);
        }

        public void Remove(Connection connection)
        {
            if (_connections.Count > 0)
                _connections.Remove(connection);
        }

        public void Remove(string connectionId)
        {
            var connection = _connections
                .FirstOrDefault(c => c.Id == connectionId);
            if (connection != null)
                _connections.Remove(connection);
        }

        public IEnumerable<Connection> GetConnections()
        {
            return _connections.ToList();
        }

        public Connection GetById(string id)
        {
            return _connections
                .FirstOrDefault(c => c.Id == id);
        }
    }
}