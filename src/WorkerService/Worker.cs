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
                var response = await _sqsClient.ReceiveMessageAsync(_config.QueueUrl);

                foreach (var message in response.Messages)
                {
                    try
                    {
                        var mensagemPedidoDto = JsonSerializer.Deserialize<MensagemPedidoDto>(message.Body);
                        if(mensagemPedidoDto == null)
                        {
                            throw new JsonException("Não foi possivel deserializar a mensagem.");
                        }

                        await _pedidoHandler.ProcessarPedidoAsync(mensagemPedidoDto);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message {messageId} with body: {body}", message?.MessageId, message?.Body);
                        
                        if (message != null)
                            await _sqsClient.SendMessageAsync(_config.DlqQueueUrl, message.Body);
                    }
                    finally
                    {
                        if (message != null)
                            await _sqsClient.DeleteMessageAsync(_config.QueueUrl, message);
                    }
                }

                await Task.Delay(1000);
            }
        }
    }
}