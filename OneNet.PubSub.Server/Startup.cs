using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OneNet.PubSub.Server.Application.Repository;
using OneNet.PubSub.Server.Infrastructures.Api.Handlers;
using OneNet.PubSub.Server.Infrastructures.SignalR;
using OneNet.PubSub.Server.Infrastructures.SignalR.Filters;
using OneNet.PubSub.Server.Infrastructures.SignalR.Hubs;
using TopicRepository = OneNet.PubSub.Server.Infrastructures.Data.Repository.TopicRepository;

namespace OneNet.PubSub.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OneNet.PubSub.Server", Version = "v1" });
            });
            services.AddSignalR(options => { options.AddFilter<HubExceptionFilter>(); });
            services.AddSingleton<ITopicRepository, TopicRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OneNet.PubSub.Server v1"));
            }

            loggerFactory.AddLog4Net();
            app.AddApiExceptionHandler(loggerFactory.CreateLogger("Global-Exception-Handler"));
            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(config => config.AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(host => true)
                .AllowCredentials());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("OneNet.PubSub.Server"));
                endpoints.MapHub<PubSubHub>(BaseHub.GetName<PubSubHub>());
            });
            // SignalR Infrastructure.
            HubConnectionManagerPool.Instance.Init();
        }
    }
}