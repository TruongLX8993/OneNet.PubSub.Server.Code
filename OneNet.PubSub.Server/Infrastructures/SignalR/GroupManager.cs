using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class GroupManagerFactory
    {
        private static GroupManagerFactory _instance;
        public static GroupManagerFactory Instance => _instance ??= new GroupManagerFactory();
        private readonly IDictionary<Type, GroupManager> _managers;

        private GroupManagerFactory()
        {
            _managers = new ConcurrentDictionary<Type, GroupManager>();
        }

        public GroupManager Get(BaseHub hub)
        {
            var type = hub.GetType();
            if (!_managers.ContainsKey(type))
            {
                _managers.Add(type, new GroupManager());
            }

            return _managers[type]
                .UpdateCurrentHub(hub);
        }
    }

    public class GroupManager
    {
        private BaseHub _baseHub;
        private readonly IDictionary<string, GroupProxy> _signalRGroupProxies;

        public GroupManager()
        {
            _signalRGroupProxies = new Dictionary<string, GroupProxy>();
        }

        public GroupManager UpdateCurrentHub(BaseHub hub)
        {
            _baseHub = hub;
            return this;
        }

        public GroupProxy GetGroupProxy(string groupName)
        {
            if (!_signalRGroupProxies.ContainsKey(groupName))
                _signalRGroupProxies.Add(groupName, new GroupProxy());
            var groupProxy = _signalRGroupProxies[groupName];
            groupProxy.UpdateHub(_baseHub);
            return groupProxy;
        }
    }
}