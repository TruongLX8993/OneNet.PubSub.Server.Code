using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OneNet.PubSub.Server.Exceptions;

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
                _logger.LogError(hubException, $"{hubMethod}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{hubMethod}");
                throw ;
            }
        }
    }
}