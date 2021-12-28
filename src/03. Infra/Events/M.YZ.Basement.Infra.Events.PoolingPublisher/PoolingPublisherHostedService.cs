using M.YZ.Basement.Infra.Events.Outbox;
using M.YZ.Basement.Utilities.Configurations;
using M.YZ.Basement.Utilities.Services.MessageBus;
using Microsoft.Extensions.Hosting;

namespace M.YZ.Basement.Infra.Events.PoolingPublisher;
public class PoolingPublisherHostedService : IHostedService
{
    private readonly BasementConfigurationOptions _configuration;
    private readonly IOutBoxEventItemRepository _outBoxEventItemRepository;
    private readonly ISendMessageBus _messageBus;
    private Timer _timer;
    public PoolingPublisherHostedService(BasementConfigurationOptions configuration, IOutBoxEventItemRepository outBoxEventItemRepository, ISendMessageBus messageBus)
    {
        _configuration = configuration;
        _outBoxEventItemRepository = outBoxEventItemRepository;
        _messageBus = messageBus;

    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SendOutBoxItems, null, TimeSpan.Zero, TimeSpan.FromSeconds(_configuration.PoolingPublisher.SendOutBoxInterval));
        return Task.CompletedTask;
    }

    private void SendOutBoxItems(object state)
    {
        _timer.Change(Timeout.Infinite, 0);
        var outboxItems = _outBoxEventItemRepository.GetOutBoxEventItemsForPublishe(_configuration.PoolingPublisher.SendOutBoxCount);

        foreach (var item in outboxItems)
        {
            _messageBus.Send(new Parcel
            {
                CorrelationId = item.AggregateId,
                MessageBody = item.EventPayload,
                MessageId = item.EventId.ToString(),
                MessageName = item.EventName,
                Route = $"{_configuration.ServiceId}.{item.EventName}",
                Headers = new Dictionary<string, object>
                {
                    ["AccuredByUserId"] = item.AccuredByUserId,
                    ["AccuredOn"] = item.AccuredOn.ToString(),
                    ["AggregateName"] = item.AggregateName,
                    ["AggregateTypeName"] = item.AggregateTypeName,
                    ["EventTypeName"] = item.EventTypeName,
                }
            });
            item.IsProcessed = true;
        }
        _outBoxEventItemRepository.MarkAsRead(outboxItems);
        _timer.Change(0, _configuration.PoolingPublisher.SendOutBoxInterval);

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}