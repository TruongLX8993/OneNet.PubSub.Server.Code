using Microsoft.AspNetCore.SignalR;

namespace OneNet.PubSub.Server.Hubs
{
    public class BaseHub : Hub
    {
        protected BaseHub(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}