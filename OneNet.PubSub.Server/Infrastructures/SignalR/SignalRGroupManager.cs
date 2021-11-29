using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class SignalRGroupManagerFactory
    {
        private static SignalRGroupManagerFactory _instance;
        public static SignalRGroupManagerFactory Instance => _instance ??= new SignalRGroupManagerFactory();
        private readonly IDictionary<Type, SignalRGroupManager> _managers;

        private SignalRGroupManagerFactory()
        {
            _managers = new ConcurrentDictionary<Type, SignalRGroupManager>();
        }

        public SignalRGroupManager Get(BaseHub hub)
        {
            var type = hub.GetType();
            if (!_managers.ContainsKey(type))
            {
                _managers.Add(type, new SignalRGroupManager());
            }

            return _managers[type]
                .UpdateCurrentHub(hub);
        }
    }

    public class SignalRGroupManager
    {
        private BaseHub _baseHub;
        private readonly IDictionary<string, SignalRGroupProxy> _signalRGroupProxies;

        public SignalRGroupManager()
        {
            _signalRGroupProxies = new Dictionary<string, SignalRGroupProxy>();
        }

        public SignalRGroupManager UpdateCurrentHub(BaseHub hub)
        {
            _baseHub = hub;
            return this;
        }

        public SignalRGroupProxy GetGroupProxy(string groupName)
        {
            if (!_signalRGroupProxies.ContainsKey(groupName))
                _signalRGroupProxies.Add(groupName, new SignalRGroupProxy());
            var groupProxy = _signalRGroupProxies[groupName];
            groupProxy.UpdateHub(_baseHub);
            return groupProxy;
        }
    }
}