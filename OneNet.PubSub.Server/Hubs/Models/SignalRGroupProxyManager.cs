using System.Collections.Generic;
using System.Linq;

namespace OneNet.PubSub.Server.Hubs.Models
{
    public class SignalRGroupProxyManager
    {
        private static SignalRGroupProxyManager _instance;

        public static SignalRGroupProxyManager Instance =>
            _instance ??= new SignalRGroupProxyManager();

        private readonly IDictionary<string, SignalRGroupProxy> _dictionary;

        private SignalRGroupProxyManager()
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