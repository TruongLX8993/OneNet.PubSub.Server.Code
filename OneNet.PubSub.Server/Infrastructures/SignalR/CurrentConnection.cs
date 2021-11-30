using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class CurrentConnection : ICurrentConnection
    {
        private readonly BaseHub _baseHub;

        public CurrentConnection(BaseHub baseHub)
        {
            _baseHub = baseHub;
        }

        public Connection GetConnection()
        {
            return _baseHub.GetCurrentConnection();
        }
    }
}