using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HackathonPosTech.Infra.Messages;

public class MessagePublisher : IMessagePublisher
{
    const string uploadQueue = "upload";
    const string emailQueue = "email";

    readonly ServiceBusSender _uploadQueueSender;
    readonly ServiceBusSender _emailQueueSender;
    readonly ServiceBusAdministrationClient _sbusAdminClient;

    public MessagePublisher(IConfiguration configuration)
    {
        var serviceBusConnectionString = configuration["ServiceBusConnectionString"];
        var sbusClient = new ServiceBusClient(serviceBusConnectionString);

        _uploadQueueSender = sbusClient.CreateSender(uploadQueue);
        _emailQueueSender = sbusClient.CreateSender(emailQueue);
        _sbusAdminClient = new ServiceBusAdministrationClient(serviceBusConnectionString);
    }

    public async Task QueueUploadMessageAsync(UploadMessageDto content)
    {
        if (!await _sbusAdminClient.QueueExistsAsync(uploadQueue))
            return;

        var messageServiceBus = new ServiceBusMessage(JsonSerializer.Serialize(content));
        await _uploadQueueSender.SendMessageAsync(messageServiceBus);
    }

    public async Task QueueEmailMessageAsync(EmailMessageDto content)
    {
        if (!await _sbusAdminClient.QueueExistsAsync(emailQueue))
            return;

        var messageServiceBus = new ServiceBusMessage(JsonSerializer.Serialize(content));
        await _emailQueueSender.SendMessageAsync(messageServiceBus);
    }
}