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

    //Todo Add Stream in Feature

    public async Task<TCommandResult> Create<TCommand, TCommandResult>(TCommand command) where TCommand : class, ICommand<TCommandResult>
    {
        var result = await _commandDispatcher.Send<TCommand, TCommandResult>(command);

        return await RpcResponseHandler(result);
    }

    public async Task<Empty> Create<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await _commandDispatcher.Send(command);

        return await RpcResponseHandler(result);
    }


    protected async Task<TCommandResult> Edit<TCommand, TCommandResult>(TCommand command) where TCommand : class, ICommand<TCommandResult>
    {
        var result = await _commandDispatcher.Send<TCommand, TCommandResult>(command);
        var message = MessagesToMessage(result.Messages);

        return await RpcResponseHandler(result);
    }

    protected async Task<Empty> Edit<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await _commandDispatcher.Send(command);
        var message = MessagesToMessage(result.Messages);

        return await RpcResponseHandler(result);
    }


    protected async Task<TCommandResult> Delete<TCommand, TCommandResult>(TCommand command) where TCommand : class, ICommand<TCommandResult>
    {
        var result = await _commandDispatcher.Send<TCommand, TCommandResult>(command);

        return await RpcResponseHandler(result);
    }

    protected async Task<Empty> Delete<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await _commandDispatcher.Send(command);
        var message = MessagesToMessage(result.Messages);

        return await RpcResponseHandler(result);
    }

    public async Task<TQueryResult> Query<TQuery, TQueryResult>(TQuery query)
        where TQuery : class, IQuery<TQueryResult>
    {
        var result = await _queryDispatcher.Execute<TQuery, TQueryResult>(query);
        var message = MessagesToMessage(result.Messages);

        if (result.Status == ApplicationServiceStatus.NotFound || result.Data == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, message));
        }

        else if (result.Status == ApplicationServiceStatus.Ok)
            return result.Data;

        throw BadRequest(message);
    }


    private Task<Empty> RpcResponseHandler(CommandResult response)
    {
        var status = response.Status;
        var message = MessagesToMessage(response.Messages);

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

    private Task<TCommandResult> RpcResponseHandler<TCommandResult>(CommandResult<TCommandResult> result)
    {
        var status = result.Status;
        var data = result.Data;
        var message = MessagesToMessage(result.Messages);

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

    private string MessagesToMessage(IEnumerable<string> messages)
    {
        var message = string.Join("\n", messages);

        return message;
    }
}