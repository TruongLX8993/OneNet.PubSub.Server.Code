using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OneNet.PubSub.Server.Application.Exceptions;

namespace OneNet.PubSub.Server.Infrastructures.Api.Handlers
{
    public static class ApiExceptionHandler
    {
        public static void AddApiExceptionHandler(this IApplicationBuilder appBuilder, ILogger logger)
        {
            appBuilder.UseExceptionHandler(app =>
            {
                app.Run(async ctx =>
                {
                    var exception = ctx.Features.Get<IExceptionHandlerFeature>()
                        ?.Error;
                    if (exception == null)
                        return;
                    logger.LogError(exception, exception.Message);
                    ApiResponse apiResponse = null;
                    if (exception is ApplicationException applicationException)
                        apiResponse = ApiResponse.CreateError(applicationException);
                    else
                        apiResponse = ApiResponse.CreateServerInternalError();
                    ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                    await ctx.Response.WriteAsJsonAsync(apiResponse);
                });
            });
        }
    }
}