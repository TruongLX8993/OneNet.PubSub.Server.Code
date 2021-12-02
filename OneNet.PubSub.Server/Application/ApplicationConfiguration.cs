using Microsoft.Extensions.DependencyInjection;
using OneNet.PubSub.Server.Application.Services;

namespace OneNet.PubSub.Server.Application
{
    public static class ApplicationConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ITopicSearchingService, TopicSearchingService>();
            services.AddTransient<ITopicService, TopicService>();
        }
    }
}