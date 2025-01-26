#!/bin/bash

# Wait for SQL Server to be ready (more robust than sleep)
until /opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "SELECT 1" > /dev/null 2>&1; do
  echo "Waiting for SQL Server to start..."
  sleep 1
done

/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "CREATE DATABASE PedidoDB;"
/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "USE PedidoDB; CREATE TABLE [dbo].[Pedido] ( Id UNIQUEIDENTIFIER PRIMARY KEY, DataCriacao DATETIME2(7) NOT NULL, UltimaAtualizacao DATETIME2(7) NOT NULL, StatusPedido INT NOT NULL );"

echo "Database initialization complete."

# kill the container
exit 0