#!/bin/sh

echo "Running LocalStack initialization..."

awslocal sqs create-queue --queue-name atualizar-pedido.fifo --attributes 'FifoQueue=true, ContentBasedDeduplication=true' 
awslocal sqs create-queue --queue-name atualizar-pedido-dlq

echo "LocalStack initialization complete."
