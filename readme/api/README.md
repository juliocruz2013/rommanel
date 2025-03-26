
# 🧾 Cadastro de Cliente - API (.NET 8)

Este projeto é a API do sistema de cadastro de clientes. Utiliza .NET 8 com arquitetura DDD, CQRS, Event Sourcing e validações com FluentValidation.

## ▶️ Execução

```bash
cd api
dotnet restore
dotnet ef database update
dotnet run
```

Acesse: http://localhost:http://localhost:6584/docs

## 🔌 Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET    | /api/customers | Lista todos os clientes |
| GET    | /api/customers/{id} | Retorna um cliente |
| POST   | /api/customers | Cria um novo cliente |
| PUT    | /api/customers/{id} | Atualiza um cliente |
| DELETE | /api/customers/{id} | Remove um cliente |
