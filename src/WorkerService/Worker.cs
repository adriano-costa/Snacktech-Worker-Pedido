using WorkerService.DTOs;
using WorkerService.Infrastructure;
using WorkerService.Handlers;
using System.Text.Json;
using WorkerService.Configuration;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISqsClient _sqsClient;
        private readonly IPedidoHandler _pedidoHandler;
        private readonly AppSettingsConfig _config;

        public Worker(
            ILogger<Worker> logger,
            ISqsClient sqsClient,
            IPedidoHandler pedidoHandler,
            AppSettingsConfig config)
        {
            _logger = logger;
            _sqsClient = sqsClient;
            _pedidoHandler = pedidoHandler;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(_config.Aws.QueueUrl);

                foreach (var message in response.Messages)
                {
                    _logger.LogDebug("Processing message {messageId}", message?.MessageId);

                    try
                    {
                        var mensagemPedidoDto = JsonSerializer.Deserialize<MensagemPedidoDto>(message!.Body);
                        if(mensagemPedidoDto == null)
                        {
                            throw new JsonException("Invalid message body");
                        }

                        await _pedidoHandler.ProcessarPedidoAsync(mensagemPedidoDto);
                        _logger.LogDebug("Message {messageId} processed with body: {body}", message?.MessageId, message?.Body);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message {messageId} with body: {body}", message?.MessageId, message?.Body);
                        
                        if (message != null){
                            await _sqsClient.SendMessageAsync(_config.Aws.DlqQueueUrl, message.Body);
                            _logger.LogWarning("Message {messageId} sent to DLQ {queueDlq}", message.MessageId, _config.Aws.DlqQueueUrl);
                        }
                    }
                    finally
                    {
                        if (message != null){
                            await _sqsClient.DeleteMessageAsync(_config.Aws.QueueUrl, message);
                            _logger.LogDebug("Message {messageId} deleted from queue {queue}", message.MessageId, _config.Aws.QueueUrl);
                        }
                    }
                }

                _logger.LogDebug("Waiting for messages...");
                await Task.Delay(5000);
            }
        }
    }
}