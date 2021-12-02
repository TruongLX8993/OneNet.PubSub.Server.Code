using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OneNet.PubSub.Server.Application.Domains
{
    public class ConnectionList
    {
        private readonly IDictionary<string, Connection> _connections;

        public ConnectionList()
        {
            _connections = new ConcurrentDictionary<string, Connection>();
        }

        public int Count => _connections.Count;

        public void Add(Connection connection)
        {
            if (!_connections.ContainsKey(connection.Id))
                _connections.Add(connection.Id, connection);
        }

        public void Remove(Connection connection)
        {
            if (_connections.ContainsKey(connection.Id))
                _connections.Remove(connection.Id, out _);
        }

        public void Remove(string connectionId)
        {
            _connections.Remove(connectionId, out _);
        }

        public IEnumerable<Connection> GetConnections()
        {
            return _connections.Values.ToList();
        }

        public Connection GetById(string id)
        {
            return _connections.ContainsKey(id) ? _connections[id] : null;
        }
    }
}