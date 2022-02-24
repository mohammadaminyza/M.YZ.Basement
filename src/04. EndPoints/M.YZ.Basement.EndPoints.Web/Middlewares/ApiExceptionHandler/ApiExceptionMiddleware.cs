using M.YZ.Basement.Utilities.Services.Logger;
using M.YZ.Basement.Utilities.Services.Serializers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace M.YZ.Basement.EndPoints.Web.Middlewares.ApiExceptionHandler
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly ApiExceptionOptions _options;
        private readonly IJsonSerializer _serializer;
        private readonly BasementConfigurationOptions _basementConfigurationOptions;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger, ApiExceptionOptions options, IJsonSerializer serializer, BasementConfigurationOptions basementConfigurationOptions)
        {
            _next = next;
            _logger = logger;
            _options = options;
            _serializer = serializer;
            _basementConfigurationOptions = basementConfigurationOptions;
        }

        public async Task Invoke(HttpContext context, IScopeInformation scopeInfo /* other dependencies */)
        {
            using IDisposable hostScope = _logger.BeginScope(scopeInfo.HostScopeInfo);
            using IDisposable requestScope = _logger.BeginScope(scopeInfo.RequestScopeInfo);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleRestExceptionAsync(context, ex);
            }
            finally
            {
                hostScope.Dispose();
                requestScope.Dispose();
            }
        }

        private Task HandleRestExceptionAsync(HttpContext context, Exception exception)
        {
            var error = new ApiError
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occurred in the API.  Please use the id and contact our " +
                        "support team if the problem persists."
            };

            _options.AddResponseDetails?.Invoke(context, exception, error);

            var innerExMessage = exception.GetInnermostExceptionMessage();

            var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;
            _logger.Log(level, exception, "BADNESS!!! " + innerExMessage + " -- {ErrorId}.", error.Id);

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
