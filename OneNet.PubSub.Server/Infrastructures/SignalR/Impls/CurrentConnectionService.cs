using OneNet.PubSub.Server.Application.Domains;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Impls
{
    public class CurrentConnectionService : ICurrentConnectionService
    {
        private BaseHub _baseHub;

        public void UpdateConnectionSource(BaseHub baseHub)
        {
            _baseHub = baseHub;
        }

        public Connection GetConnection()
        {
            return _baseHub.GetCurrentConnection();
        }
    }
}