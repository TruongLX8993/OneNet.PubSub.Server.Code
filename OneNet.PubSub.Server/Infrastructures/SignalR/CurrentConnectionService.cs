using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public class CurrentConnectionService : ICurrentConnection
    {
        private readonly BaseHub _baseHub;

        public CurrentConnectionService(BaseHub baseHub)
        {
            _baseHub = baseHub;
        }

        public Connection GetConnection()
        {
            return _baseHub.GetCurrentConnection();
        }
    }
}