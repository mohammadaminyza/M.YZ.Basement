using M.YZ.Basement.Core.ApplicationServices.Events;
using M.YZ.Basement.Utilities.Configurations;
using M.YZ.Basement.Utilities.Services.MessageBus;
using M.YZ.Basement.Utilities.Services.Serializers;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;

namespace M.YZ.Basement.Messaging.IdempotentConsumers;
public class IdempotentMessageConsumer : IMessageConsumer
{
    private readonly BasementConfigurationOptions _basementConfigurations;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageInboxItemRepository _messageInboxItemRepository;
    private readonly Dictionary<string, string> _messageTypeMap = new Dictionary<string, string>();
    public IdempotentMessageConsumer(BasementConfigurationOptions basementConfigurations, IEventDispatcher eventDispatcher, IJsonSerializer jsonSerializer, ICommandDispatcher commandDispatcher, IMessageInboxItemRepository messageInboxItemRepository)
    {
        _basementConfigurations = basementConfigurations;
        _eventDispatcher = eventDispatcher;
        _jsonSerializer = jsonSerializer;
        _commandDispatcher = commandDispatcher;
        _messageInboxItemRepository = messageInboxItemRepository;
        LoadMessageMap();
    }

    private void LoadMessageMap()
    {

        if (_basementConfigurations?.Messageconsumer?.Events?.Any() == true)
        {
            foreach (var eventPublisher in _basementConfigurations?.Messageconsumer?.Events)
            {
                foreach (var @event in eventPublisher?.EventData)
                {
                    _messageTypeMap.Add($"{eventPublisher.FromServiceId}.{@event.EventName}", @event.MapToClass);

                }
            }
        }
        if (_basementConfigurations?.Messageconsumer?.Commands?.Any() == true)
        {
            foreach (var item in _basementConfigurations?.Messageconsumer?.Commands)
            {
                _messageTypeMap.Add($"{_basementConfigurations.ServiceId}.{item.CommandName}", item.MapToClass);
            }
        }
    }

    public void ConsumeCommand(string sender, Parcel parcel)
    {
        if (_messageInboxItemRepository.AllowReceive(parcel.MessageId, sender))
        {
            var mapToClass = _messageTypeMap[parcel.Route];
            var commandType = Type.GetType(mapToClass);
            dynamic command = _jsonSerializer.Deserialize(parcel.MessageBody, commandType);
            _commandDispatcher.Send(command);
            _messageInboxItemRepository.Receive(parcel.MessageId, sender);
        }
    }

    public void ConsumeEvent(string sender, Parcel parcel)
    {
        if (_messageInboxItemRepository.AllowReceive(parcel.MessageId, sender))
        {
            var mapToClass = _messageTypeMap[parcel.Route];
            var eventType = Type.GetType(mapToClass);
            dynamic @event = _jsonSerializer.Deserialize(parcel.MessageBody, eventType);
            _eventDispatcher.PublishDomainEventAsync(@event);
            _messageInboxItemRepository.Receive(parcel.MessageId, sender);
        }
    }
}
