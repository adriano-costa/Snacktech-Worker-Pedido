# Specification for a Worker Service using AWS FIFO SQS and Dead Letter Queue (DLQ) Pattern

1.  Objectives
    The worker service aims to process messages from an Amazon SQS FIFO queue, ensuring each message is handled in order. It should efficiently manage successful processing with acknowledgments and route unprocessable messages to a DLQ.

2.  Functional Requirements
    2.1 Message Retrieval
    The service must retrieve messages from the specified FIFO SQS queue.
    Messages are processed sequentially as per FIFO requirements.
    The message follows this DTO format:

         public class MensagemPedidoDto
         {
             public Guid PedidoId { get; set; }
             public DateTime DataModificacao { get; set; }
             public int StatusPedido { get; set; }
             public string Mensagem { get; set; }
         }

    2.2 Processing Logic
    Each message is deserialized into a .NET object and passed to a handler for processing.
    Asynchronous handling of each message ensures scalability.
    The values of StatusPedido and DataModificacao in the message will be used to update the StatusPedido and UltimaAtualizacao in the table.
    This class describes the database table:

         public class Pedido
         {
             public Guid Id { get; set; }
             public DateTime DataCriacao { get; set; }
             public DateTime UltimaAtualizacao { get; set; }
             public Cliente Cliente { get; set; } = new();
             public StatusPedido Status { get; set; }
             public List<PedidoItem> Itens { get; set; } = [];
         }

    Reject the update and send the message to DLQ if the DataModificacao of the message was before UltimaAtualizacao of the current value in the table.
    Reject the update and send the message to DLQ if the PedidoID of the message does not match with any Id in the database.
    
    2.3 Dead Letter Queue Handling
    Upon unprocessable messages, the worker should move them to a designated DLQ.
    DLQ messages include metadata like the original queue name, timestamp, and error details.

3.  Non-Functional Requirements
    3.1 Performance
    The service must handle high throughput efficiently.
    Latency should be minimized for timely processing.
    Reliability is paramount with no message loss during failure recovery.
    3.2 Scalability
    Designed to scale horizontally to manage varying workloads.
4.  Design Considerations
    4.1 FIFO Queue Processing
    Ensures messages are processed in exact order of arrival.
    4.2 Asynchronous Processing
    Implement message processing asynchronously to allow concurrent handling, enhancing scalability.
    4.3 DLQ Integration
    Messages failing processing are sent to DLQ with appropriate metadata for review.
5.  Technologies and Tools
    5.1 Core Components
    AWS SDK for .NET: For SQS interactions.
    Custom Message Class: To encapsulate message content, type, ID, timestamp, and DLQ info.
    Handler Interface: Standardizes processing logic across handlers.
6.  Error Handling
    6.1 Error Types
    Deserialization Errors: When converting from string to object fails.
    Invalid Data: Data incompatible with the expected format.
    Processing Logic Errors: Exceptions thrown during handler execution.
    6.2 Error Handling Process
    Log exceptions and error details.
    Move message to DLQ if unprocessable, ensuring the original queue isn't blocked.
7.  Logging
    Use a logging library (e.g., Serilog) for detailed logs.
    Capture exceptions, processing times, success/failure status, queue name, and message ID.
8.  Monitoring
    8.1 Metrics
    Track messages processed per second, error rates, latency, and DLQ usage.
    8.2 Alerts
    Set up notifications for high error rates or prolonged failures to notify administrators promptly.
9.  Security Considerations
    Use IAM roles with minimal permissions (least privilege) for SQS operations.
    Ensure secure communication using HTTPS.
10. Test Coverage
    The code of the worker must have at least 80% coverage of unit tests.
    The test must be placed in a separate Class Library project.

Implementation Note: The worker service should be structured with clear separation of concerns, using dependency injection for flexibility. Ensure proper exception handling and logging to maintain reliability and facilitate troubleshooting.
