using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ApplicationException = OneNet.PubSub.Server.Application.Exceptions.ApplicationException;

namespace OneNet.PubSub.Server.Infrastructures.SignalR.Filters
{
    public class HubExceptionFilter : IHubFilter
    {
        private readonly ILogger<HubExceptionFilter> _logger;

        public HubExceptionFilter(ILogger<HubExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            var hubMethod = invocationContext.HubMethod;
            try
            {
                return await next(invocationContext);
            }
            catch (HubException e)
            {
                _logger.LogError(e, $"Hub method: {hubMethod}");
                throw;
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e, $"Hub method: {hubMethod}");
                throw new HubException(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Hub method: {hubMethod}");
                throw;
            }
        }
    }
}