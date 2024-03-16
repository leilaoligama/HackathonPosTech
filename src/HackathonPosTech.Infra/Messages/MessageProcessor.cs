using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace HackathonPosTech.Infra.Messages;
public class MessageProcessor : IHostedService, IMessageProcessor
{
    const string uploadQueue = "processavideo";
    const string emailQueue = "notificaemail";

    readonly ServiceBusProcessor _uploadQueueProcessor;
    readonly ServiceBusProcessor _emailQueueProcessor;
    readonly ServiceBusAdministrationClient _sbusAdminClient;
    readonly IMessageProcessorService<UploadMessageDto> _userMessageProcessorService;
    readonly IMessageProcessorService<EmailMessageDto> _emailMessageProcessorService;

    public MessageProcessor(
        IConfiguration configuration, 
        IMessageProcessorService<UploadMessageDto> userMessageProcessorService,
        IMessageProcessorService<EmailMessageDto> emailMessageProcessorService)
    {
        var serviceBusConnectionString = configuration["ServiceBusConnectionString"];
        var sbusClient = new ServiceBusClient(serviceBusConnectionString);

        _sbusAdminClient = new ServiceBusAdministrationClient(serviceBusConnectionString);
        _uploadQueueProcessor = sbusClient.CreateProcessor(uploadQueue, new ServiceBusProcessorOptions { AutoCompleteMessages = false });
        _emailQueueProcessor = sbusClient.CreateProcessor(emailQueue, new ServiceBusProcessorOptions { AutoCompleteMessages = false });

        _userMessageProcessorService = userMessageProcessorService;
        _emailMessageProcessorService = emailMessageProcessorService;

        ValidateAndCreateQueue(uploadQueue).Wait();
        ValidateAndCreateQueue(emailQueue).Wait();
    }

    private async Task ValidateAndCreateQueue(string queueName)
    {
        if (!await _sbusAdminClient.QueueExistsAsync(queueName))
            await _sbusAdminClient.CreateQueueAsync(queueName);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _uploadQueueProcessor.ProcessMessageAsync += UploadMessageHandler;
        _uploadQueueProcessor.ProcessErrorAsync += ErrorHandler;
        _uploadQueueProcessor.StartProcessingAsync().Wait();

        _emailQueueProcessor.ProcessMessageAsync += EmailMessageHandler;
        _emailQueueProcessor.ProcessErrorAsync += ErrorHandler;
        _emailQueueProcessor.StartProcessingAsync().Wait();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _uploadQueueProcessor.StopProcessingAsync();
        _uploadQueueProcessor.ProcessMessageAsync -= UploadMessageHandler;
        _uploadQueueProcessor.ProcessErrorAsync -= ErrorHandler;

        await _emailQueueProcessor.StopProcessingAsync();
        _emailQueueProcessor.ProcessMessageAsync -= EmailMessageHandler;
        _emailQueueProcessor.ProcessErrorAsync -= ErrorHandler;
    }

    private async Task UploadMessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            var messageBody = args.Message.Body.ToString();
            var messageDto = JsonSerializer.Deserialize<UploadMessageDto>(messageBody);
            await _userMessageProcessorService.ProcessMessage(messageDto);

            await args.CompleteMessageAsync(args.Message);
        }
        catch
        {
            await args.DeadLetterMessageAsync(args.Message);
        }
    }

    private async Task EmailMessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            var messageBody = args.Message.Body.ToString();
            var messageDto = JsonSerializer.Deserialize<EmailMessageDto>(messageBody);
            await _emailMessageProcessorService.ProcessMessage(messageDto);

            await args.CompleteMessageAsync(args.Message);
        }
        catch
        {
            await args.DeadLetterMessageAsync(args.Message);
        }
    }

    private async Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception);
    }
}