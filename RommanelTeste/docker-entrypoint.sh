#!/bin/bash
set -x

echo "Aguardando SQL Server iniciar..."

SQLSERVER_HOST=${SQLSERVER_HOST:-sqlserver}
SQLSERVER_PORT=1433

until nc -z $SQLSERVER_HOST $SQLSERVER_PORT; do
    echo "SQL Server ainda não está pronto. Aguardando..."
    sleep 5
done

echo "SQL Server está pronto! Executando migrações..."

if [ -f /app/migrations/RommanelTeste.Persistence.Migration.dll ]; then
    dotnet /app/migrations/RommanelTeste.Persistence.Migration.dll
else
    echo "Migration não encontrada! Pulando execução."
fi

echo "Inicializando a API..."
exec dotnet /app/api/RommanelTeste.Presentation.API.dll