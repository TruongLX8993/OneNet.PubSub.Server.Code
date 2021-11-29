using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.Interfaces
{
    public interface ICurrentConnection 
    {
        public Connection GetConnection();
    }
}