namespace WorkerService.Infrastructure;

using Amazon.SQS;
using Amazon.SQS.Model;

public class SqsClient
{
    private readonly IAmazonSQS _sqsClient;

    public SqsClient(IAmazonSQS sqsClient)
    {
        _sqsClient = sqsClient;
    }

    public async Task SendMessageAsync(string queueUrl, string messageBody)
    {
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = messageBody
        };

        await _sqsClient.SendMessageAsync(request);
    }
}