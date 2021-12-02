using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class HubConnectionManagerPool
    {
        private static HubConnectionManagerPool _instance;
        public static HubConnectionManagerPool Instance => _instance ??= new HubConnectionManagerPool();
        private readonly IDictionary<Type, ConnectionManager> _connectionManagers;
        private readonly object _lock = new object();


        private HubConnectionManagerPool()
        {
            _connectionManagers = new ConcurrentDictionary<Type, ConnectionManager>();
        }

        public void Init()
        {
            // Todo
        }

        public ConnectionManager Get(BaseHub baseHub)
        {
            lock (_lock)
            {
                var type = baseHub.GetType();
                if (_connectionManagers.ContainsKey(type))
                    return _connectionManagers[type];
                _connectionManagers.TryAdd(type, new ConnectionManager());
                return _connectionManagers[type];
            }
        }
    }
}