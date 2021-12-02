using Microsoft.Extensions.DependencyInjection;
using OneNet.PubSub.Server.Application.Repository;
using OneNet.PubSub.Server.Infrastructures.Data.Repository;

namespace OneNet.PubSub.Server.Infrastructures.Data
{
    public static class DataConfiguration
    {
        public static void AddDataServices(this IServiceCollection service)
        {
            service.AddSingleton<ITopicRepository, TopicRepository>();
        }
    }
}