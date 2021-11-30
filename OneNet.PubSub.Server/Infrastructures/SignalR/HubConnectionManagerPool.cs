using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class HubConnectionManagerPool
    {
        private static HubConnectionManagerPool _instance;
        public static HubConnectionManagerPool Instance => _instance ??= new HubConnectionManagerPool();

        private readonly IDictionary<Type, ConnectionManager> _connectionManagers;

        private HubConnectionManagerPool()
        {
            _connectionManagers = new ConcurrentDictionary<Type, ConnectionManager>();
        }

        public ConnectionManager Get(BaseHub baseHub)
        {
            var type = baseHub.GetType();
            if (_connectionManagers.ContainsKey(type))
                return _connectionManagers[type];
            _connectionManagers.Add(type, new ConnectionManager());
            return _connectionManagers[type];
        }
    }
}