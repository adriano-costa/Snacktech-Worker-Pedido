#!/bin/bash

# Wait for SQL Server to be ready (more robust than sleep)
until /opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "SELECT 1" > /dev/null 2>&1; do
  echo "Waiting for SQL Server to start..."
  sleep 1
done

/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "CREATE DATABASE PedidoDB;"
/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "USE PedidoDB; CREATE TABLE [dbo].[Pedidos] ( Id UNIQUEIDENTIFIER PRIMARY KEY, DataCriacao DATETIME2(7) NOT NULL, UltimaAtualizacao DATETIME2(7) NOT NULL, Status INT NOT NULL );"

/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "USE PedidoDB; INSERT INTO [dbo].[Pedidos] (Id, DataCriacao, UltimaAtualizacao, Status) VALUES ('6c5250e1-5843-42b9-bce7-224f0cbf01a9', '2025-01-01 00:00:00', '2025-01-01 00:00:00', 0);"
/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "USE PedidoDB; INSERT INTO [dbo].[Pedidos] (Id, DataCriacao, UltimaAtualizacao, Status) VALUES ('e0882757-1bda-4884-9861-ba8d9e2e7869', '2025-01-01 00:00:00', '2025-01-01 00:00:00', 1);"
/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "USE PedidoDB; INSERT INTO [dbo].[Pedidos] (Id, DataCriacao, UltimaAtualizacao, Status) VALUES ('e099fd40-28fa-4ed2-8d69-bd7719cc45da', '2025-01-01 00:00:00', '2025-01-01 00:00:00', 3);"
/opt/mssql-tools/bin/sqlcmd -S sql-server-local -U sa -P YourStrong@Password123 -Q "USE PedidoDB; INSERT INTO [dbo].[Pedidos] (Id, DataCriacao, UltimaAtualizacao, Status) VALUES ('ff1fe84a-0998-49ec-b0aa-264796a3fc3d', '2025-01-01 00:00:00', '2025-01-01 00:00:00', 4);"

echo "Database seed complete."

echo "Database initialization complete."

# kill the container
exit 0