using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using M.YZ.Basement.Core.ApplicationServices.Events;
using M.YZ.Basement.Utilities;

namespace M.YZ.Basement.EndPoints.Web.Controllers;

public class BaseGRpcServiceController
{
    private ICommandDispatcher _commandDispatcher;
    private IQueryDispatcher _queryDispatcher;
    private IEventDispatcher _eventDispatcher;
    private BasementServices _basementApplicationContext;

    public BaseGRpcServiceController(IServiceProvider serviceProvider)
    {
        _commandDispatcher = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICommandDispatcher>();
        _queryDispatcher = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IQueryDispatcher>();
        _eventDispatcher = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEventDispatcher>();
        _basementApplicationContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<BasementServices>();
    }
    public async Task<TCommandResult> Create<TCommand, TCommandResult>(TCommand command) where TCommand : class, ICommand<TCommandResult> where TCommandResult : IMessage<TCommandResult>
    {
        var result = await _commandDispatcher.Send<TCommand, TCommandResult>(command);

        return await GRpcErrorHandler(result);
    }

    public async Task<Empty> Create<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await _commandDispatcher.Send(command);

        return await GRpcErrorHandler(result);
    }



    private Task<Empty> GRpcErrorHandler(CommandResult response)
    {
        var message = string.Join(" , ", response.Messages);
        var status = response.Status;

        if (status == ApplicationServiceStatus.Ok)
        {
            return Task.FromResult(new Empty());
        }

        else if (status == ApplicationServiceStatus.ValidationError)
        {
            throw new RpcException(new Status(StatusCode.OutOfRange, message));
        }

        else if (status == ApplicationServiceStatus.NotFound)
        {
            throw new RpcException(new Status(StatusCode.NotFound, nameof(StatusCode.NotFound)));
        }

        throw BadRequest(message);
    }

    private Task<TResponse> GRpcErrorHandler<TResponse>(CommandResult<TResponse> response) where TResponse : IMessage<TResponse>
    {
        var message = string.Join(" , ", response.Messages);
        var status = response.Status;
        var data = response.Data;

        if (status == ApplicationServiceStatus.Ok)
        {
            return Task.FromResult(data);
        }

        else if (status == ApplicationServiceStatus.ValidationError)
        {
            throw new RpcException(new Status(StatusCode.OutOfRange, message));
        }

        else if (status == ApplicationServiceStatus.NotFound)
        {
            throw new RpcException(new Status(StatusCode.NotFound, nameof(StatusCode.NotFound)));
        }

        throw BadRequest(message);
    }

    private RpcException BadRequest(string error)
    {
        throw new RpcException(new Status(StatusCode.Aborted, error));
    }
}