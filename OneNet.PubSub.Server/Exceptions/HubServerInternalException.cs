using Microsoft.AspNetCore.SignalR;

namespace OneNet.PubSub.Server.Exceptions
{
    public class HubServerInternalException : HubException
    {
        public HubServerInternalException() :base("Server internal exception")
        {
            
        }
    }
}