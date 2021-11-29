using System;

namespace OneNet.PubSub.Server.Hubs
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