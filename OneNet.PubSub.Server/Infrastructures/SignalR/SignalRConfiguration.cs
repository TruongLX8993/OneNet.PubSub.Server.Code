using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OneNet.PubSub.Server.Application.Interfaces;
using OneNet.PubSub.Server.Infrastructures.SignalR.Filters;
using OneNet.PubSub.Server.Infrastructures.SignalR.Impls;

namespace OneNet.PubSub.Server.Infrastructures.SignalR
{
    public static class SignalRConfiguration
    {
        public static void AddSignalRService(this IServiceCollection services)
        {
            services.AddSignalR(options => { options.AddFilter<HubExceptionFilter>(); });
            services.AddTransient<IMessageSender, MessageSender>();
            services.AddTransient<INotification, Notification>();
            services.AddScoped<CurrentConnectionService, CurrentConnectionService>();
            services.AddScoped<ICurrentConnectionService, CurrentConnectionService>();
            services.AddTransient<ISubscription, Subscription>();
        }

        public static void ConfigureSignalR(this IApplicationBuilder builder)
        {
            HubConnectionManagerPool.Instance.Init();
        }
    }
}