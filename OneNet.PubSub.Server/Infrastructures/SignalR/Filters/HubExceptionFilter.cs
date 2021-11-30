using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OneNet.PubSub.Server.Exceptions;
using OneNet.PubSub.Server.Hubs;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Filters
{
    public class HubExceptionFilter : IHubFilter
    {
        private readonly ILogger<HubExceptionFilter> _logger;

        public HubExceptionFilter(ILogger<HubExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
        {
            var hubMethod = invocationContext.HubMethod;
            try
            {
                return await next(invocationContext);
            }
            catch (HubException hubException)
            {
                _logger.LogError($"{hubMethod}", hubException);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{hubMethod}", ex);
                throw new HubServerInternalException();
            }
        }
    }
}