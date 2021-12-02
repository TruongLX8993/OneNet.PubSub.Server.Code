using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class GroupManagerFactory
    {
        private static GroupManagerFactory _instance;
        public static GroupManagerFactory Instance => _instance ??= new GroupManagerFactory();
        private readonly IDictionary<Type, GroupManagerProxy> _managers;

        private GroupManagerFactory()
        {
            _managers = new ConcurrentDictionary<Type, GroupManagerProxy>();
        }

        public GroupManagerProxy Get<T>(IHubContext<T> hubContext) where T : Hub
        {
            var type = typeof(T);
            if (!_managers.ContainsKey(type))
            {
                _managers.Add(type, new GroupManagerProxy());
            }

            return _managers[type]
                .UpdateGroupManager(hubContext.Groups);
        }

        public GroupManagerProxy Get(BaseHub baseHub)
        {
            var type = baseHub.GetType();
            if (!_managers.ContainsKey(type))
            {
                _managers.Add(type, new GroupManagerProxy());
            }

            return _managers[type]
                .UpdateGroupManager(baseHub.Groups);
        }
    }

    /// <summary>
    /// Manage group in hubs.
    /// </summary>
    public class GroupManagerProxy
    {
        private IGroupManager _groupManager;
        private readonly IDictionary<string, GroupProxy> _signalRGroupProxies;

        public GroupManagerProxy()
        {
            _signalRGroupProxies = new Dictionary<string, GroupProxy>();
        }

        public GroupManagerProxy UpdateGroupManager(IGroupManager groupManager)
        {
            _groupManager = groupManager;
            return this;
        }

        public GroupProxy GetGroupProxy(string groupName)
        {
            if (!_signalRGroupProxies.ContainsKey(groupName))
                _signalRGroupProxies.Add(groupName, new GroupProxy(groupName));
            var groupProxy = _signalRGroupProxies[groupName];
            groupProxy.UpdateGroupManager(_groupManager);
            return groupProxy;
        }
    }
}