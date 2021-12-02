using System;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Hubs
{
    public class HubNameAttr : Attribute
    {
        public string Name { get; }

        public HubNameAttr(string name)
        {
            Name = name;
        }
        
    }
}