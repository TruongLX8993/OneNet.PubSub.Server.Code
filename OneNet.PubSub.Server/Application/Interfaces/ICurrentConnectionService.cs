using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.Interfaces
{
    public interface ICurrentConnectionService 
    {
        public Connection GetConnection();
    }
}