# 🧾 CRUD Cadastro de Cliente - Projeto Fullstack  

Sistema completo com Angular 16 e .NET 8 para cadastro de clientes. 
A aplicação segue boas práticas como DDD, CQRS, Mediator, FluentValidation, Event Sourcing, 
Clean Architecture, Migrations, testes automatizados e orquestração via Docker.

---

## 📁 Estrutura

- `/RommanelTeste` → Back-end com .NET 8
- `/RommanelTesteFront` → Front-end com Angular 16 standalone

---

## 🚀 Tecnologias

### Back-end (.NET 8)
- ASP.NET Core 8
- DDD (Domain-Driven Design)
- CQRS com MediatR
- Entity Framework Core
- Clean Architecture
- Event Sourcing (conceitual)
- FluentValidation
- Migrations
- xUnit
- Docker Compose

### Front-end (Angular 16)
- Angular standalone components
- Reactive Forms
- HttpClient
- Angular Router
- CSS puro
- Bootstrap (opcional)

---

## 🧩 Funcionalidades

- ✅ Listagem de cliente
- ✅ Cadastro de cliente
- ✅ Edição de cliente
- ✅ Exclusão de cliente
- ✅ Formulário com regras dinâmicas
- ✅ Validações de formulário
- ✅ Testes unitários no back-end
- ✅ Docker Compose para orquestração

---

## 🐳 Docker

```bash
docker-compose up --build
```

- API: http://localhost:6584/docs  
- Front: http://localhost:4200

---

## 🔧 Como executar manualmente

### Pré-requisitos

- .NET 8 SDK
- Node.js 18+
- Angular CLI
- Docker e Docker Compose
- SQL Server (Docker já provê)

---

## ▶️ Execução manual

### API (.NET 8)

```bash
cd api
dotnet restore
dotnet ef database update
dotnet run
```

Acesse: http://localhost:6584/docs

### Front-end (Angular 16)

```bash
cd front
npm install
ng serve
```

Acesse: http://localhost:4200

## ▶️ Execução Manual e Teste de API via SWAGGER (Sem execução do Migrations)

Acesse: http://localhost:6584/docs

- Executar scrip (script-banco.sql) - SQL SERVER 
- Ao executar a API, deve se autenticar no sistema no endpoint http://localhost:6584/api/v1/authentication
- Após se autenticar na API, deve copiar o token jwt gerado e enviar no authorization Exemplo: "Bearer token" 
- É possível criar um novo usuário de sistema para se autenticar na API no endpoint http://localhost:6584/api/v1/users

---

## 🔌 Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| GET    | `/api/v1/customers` | Lista todos os clientes |
| GET    | `/api/v1/customers/id/{id}` | Retorna um cliente por ID |
| POST   | `/api/v1/customers` | Cria um novo cliente |
| PUT    | `/api/v1/customers` | Atualiza um cliente |
| DELETE | `/api/v1/customers/id/{id}` | Remove um cliente |
| POST   | `/api/v1/authentication` | Autentica usuário na API |
| POST   | `/api/v1/users` | Cria usuário de sistema para autenticação na API |

### 📝 Payload de Autenticação

```json
{
  "email": "teste@teste.com",
  "password": "Teste@123"
}
```

### 📝 Payload de Exemplo Criação de Usuário de Sistema

```json
{
  "email": "teste@teste.com",
  "password": "Teste@123",
  "passwordConfirmation": "Teste@123"
}
```

### 📝 Payload de exemplo de Cadastro de Cliente

```json
{
  "name": "Maria da Silva",
  "documentNumber": "085.648.846-15",
  "email": "maria@email.com",
  "birthDate": "1990-01-01",
  "phone": "(11)99999-8888",
  "isCompany": false,
  "isExempt": false,
  "stateRegistration": "605.639.149.147",
  "address": {
    "street": "Rua Exemplo",
    "number": "100",
    "city": "São Paulo",
    "state": "SP",
    "zipCode": "01000-000"
  }
}
```

---

## 🧪 Testes

### API

```bash
cd api
dotnet test
```

> Testes feitos com xUnit + FluentAssertions

---

## 📄 Formulário Angular

O formulário do cliente é dinâmico:

- Se "É empresa?" estiver marcado, exibe CNPJ e "Isento?".
- Se "Isento?" estiver marcado, esconde "Inscrição Estadual".
- Se "É empresa?" não estiver marcado, exibe CPF.

Todos os campos são obrigatórios e sem máscaras.

---

## 🛠️ Comandos úteis

### Angular

```bash
ng generate component pages/cliente/cliente
ng generate component pages/cliente/cliente
ng generate service services/cliente
```

### EF Core (API)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 📚 Swagger

A documentação da API está disponível em:
```
http://localhost:6584/docs
```

---

## 📄 Observações adicionais.

### CQRS: Motivos de utilização:
- Código mais limpo e especializado.
- Otimização de cada parte do sistema separadamente.
- Facilita o desempenho e escalabilidade.

### Mediator: Motivos de utilização:
- Redução no acoplamento entre as partes do sistema (quem envia e quem processa).
- Facilita a manutenção e testes unitários.
- Facilidade de utilização.
- 
### Clean Architecture: Motivos de utilização:
- Organização de código e estrutura do projeto.
- Deixa mais clara a função de cada camada (domain, application, infra, test, e etc).
- Facilita os testes e manutenção do sistema. 
- Ajuda na criação de software escalável, permitindo que cresça de forma controlada e organizada

### FluentValidation: Motivos de utilização:
- Organização do código e validações.
- Sintaxe fluente e expressiva.
- Facilidade para testes unitários.
- Integração com ASP.NET Core.
- Integração com MediatR (pipeline), validação automática de comandos e queries antes de chegar nos Handlers.

- ### Migrations: Motivos de utilização:
- Versionamento do banco de dados.
- Sincronização com o código.
- Facilita o deployment.
- Histórico e rastreabilidade.
- Automatização com CI/CD.

### xUnit: Motivos de utilização:
- Biblioteca recomendada pela própria Microsoft.
- Isolamento total entre testes.
- Suporte a injeção de dependência nos testes.
- Integração com ferramentas de build e CI/CD.

## 👨‍💻 Autor

**Julio Cesar Santos Felix Cruz**

[LinkedIn](https://www.linkedin.com/in/julio-cruz86/) | .NET Developer | Angular | AWS | CQRS | DDD

---

## 📝 Licença

Este projeto está sob a licença MIT.
