using System.Collections.Generic;
using System.Linq;

namespace OneNet.PubSub.Server.Hubs.Models
{
    public class SignalRGroupProxyManagerFactory
    {
        private static SignalRGroupProxyManagerFactory _instance;

        public static SignalRGroupProxyManagerFactory Instance =>
            _instance ??= new SignalRGroupProxyManagerFactory();

        private readonly IDictionary<string, SignalRGroupProxy> _dictionary;

        private SignalRGroupProxyManagerFactory()
        {
            _dictionary = new Dictionary<string, SignalRGroupProxy>();
        }

        public SignalRGroupProxy GetGroupProxy(BaseHub hub)
        {
            var hubName = hub.Name;
            if (!_dictionary.ContainsKey(hubName))
                _dictionary.Add(hubName, new SignalRGroupProxy());
            var signalRGroupProxy = _dictionary[hubName];
            signalRGroupProxy.UpdateHub(hub);
            return signalRGroupProxy;
        }
    }
}