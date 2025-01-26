#!/bin/bash

echo "Running queue seed..."

aws --endpoint-url=http://localhost:4566 sqs send-message --queue-url http://localhost:4566/000000000000/atualizar-pedido.fifo --message-body '{"PedidoId": "6c5250e1-5843-42b9-bce7-224f0cbf01a9", "DataModificacao": "2025-01-01T12:00:00Z", "StatusPedido": 1}' --message-group-id 1
aws --endpoint-url=http://localhost:4566 sqs send-message --queue-url http://localhost:4566/000000000000/atualizar-pedido.fifo --message-body '{"PedidoId": "e0882757-2bda-4884-9861-ba8d9e2e7869", "DataModificacao": "2025-01-01T12:00:00Z", "StatusPedido": 2}' --message-group-id 1
aws --endpoint-url=http://localhost:4566 sqs send-message --queue-url http://localhost:4566/000000000000/atualizar-pedido.fifo --message-body '{"PedidoId": "e099fd40-28fa-4ed2-8d69-bd7719cc45da", "DataModificacao": "2025-01-01T12:00:00Z", "StatusPedido": 3}' --message-group-id 1
aws --endpoint-url=http://localhost:4566 sqs send-message --queue-url http://localhost:4566/000000000000/atualizar-pedido.fifo --message-body '{"PedidoId": "ff1fe84a-0998-49ec-b0aa-264796a3fc3d", "DataModificacao": "2025-01-01T12:00:00Z", "StatusPedido": 0}' --message-group-id 1


echo "Queue seed complete."

# kill the container
exit 0