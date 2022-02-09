using Grpc.Core;
using Grpc.Core.Interceptors;
using Newtonsoft.Json;

namespace M.YZ.Basement.EndPoints.Web.Middlewares.ApiExceptionHandler;

public class GRpcApiExceptionHandler : Interceptor
{
    private ILogger<GRpcApiExceptionHandler> _logger;

    public GRpcApiExceptionHandler(ILogger<GRpcApiExceptionHandler> logger)
    {
        _logger = logger;
    }


    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var exceptionOptions = ApiExceptionOptions.ApiExceptionOptionDefaultSetting(new ApiExceptionOptions());

        try
        {
            return await continuation(request, context);
        }
        catch (RpcException exception)
        {
            RpcExceptionHandler(exception, exceptionOptions);
            throw;
        }
        catch (Exception exception)
        {
            await HandleGRpcExceptionAsync(exception, exceptionOptions);
            throw;
        }
    }

    private Task HandleGRpcExceptionAsync(Exception exception, ApiExceptionOptions options)
    {
        var error = new ApiError
        {
            Id = Guid.NewGuid().ToString(),
            Status = (short)StatusCode.FailedPrecondition,
            Title = "Some kind of error occurred in the API.  Please use the id and contact our " +
                    "support team if the problem persists."
        };


        var innerExMessage = exception.GetInnermostExceptionMessage();

        var level = options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;
        _logger.Log(level, exception, "BADNESS!!! " + innerExMessage + " -- {ErrorId}.", error.Id);

        var failedPreconditionError = JsonConvert.SerializeObject(error);

        throw new RpcException(new Status(StatusCode.FailedPrecondition, failedPreconditionError));
    }

    private void RpcExceptionHandler(RpcException exception, ApiExceptionOptions options)
    {
        if (IsNotFound(exception))
        {
            throw new RpcException(new Status(StatusCode.OutOfRange, exception.Status.Detail));
        }

        else if (IsValidationError(exception))
        {
            throw new RpcException(new Status(StatusCode.OutOfRange, exception.Status.Detail));
        }

        HandleGRpcExceptionAsync(exception, options);
    }

    private bool IsNotFound(RpcException exception) =>
       exception.Message.Contains("NotFound");

    private bool IsValidationError(RpcException exception) =>
         exception.Message.Contains("OutOfRange");
}