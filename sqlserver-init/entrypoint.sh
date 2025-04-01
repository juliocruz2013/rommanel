#!/bin/bash

echo "Iniciando SQL Server em segundo plano..."
/opt/mssql/bin/sqlservr &

echo "Aguardando SQL Server iniciar (sleep inicial)..."
sleep 15

SA_PASSWORD=${SA_PASSWORD:-"Kbck6!p8RS"}

until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1
do
    echo "Ainda não conectado ao SQL Server... tentando novamente em 5s..."
    sleep 60
done

echo "Conectado ao SQL Server!"

if [ -f /init/init.sql ]; then
    echo "Executando script de inicialização do banco..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /init/init.sql

    if [ $? -eq 0 ]; then
        echo "Script executado com sucesso!"
    else
        echo "Erro ao executar script de inicialização"
    fi
else
    echo "Nenhum script de inicialização encontrado."
fi

echo "SQL Server ativo. Entrando em modo wait..."
wait
