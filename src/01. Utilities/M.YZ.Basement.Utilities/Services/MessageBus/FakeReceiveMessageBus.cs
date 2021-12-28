﻿using Microsoft.Extensions.Logging;

namespace M.YZ.Basement.Utilities.Services.MessageBus;

public class FakeReceiveMessageBus : IReceiveMessageBus
{
    private readonly ILogger<FakeSendMessageBus> _logger;

    public FakeReceiveMessageBus(ILogger<FakeSendMessageBus> logger)
    {
        _logger = logger;
    }

    public void Receive(string commandName)
    {
        _logger.LogInformation("fake message bus receive {commandName}", commandName);
    }

    public void Subscribe(string serviceId, string eventName)
    {
        _logger.LogInformation("fake message bus subscribe for event: {eventName} from service {serviceId}", eventName, serviceId);
    }
}

