# Nonfy Backend

Backend da aplicação Nonfy, construído com .NET 9, Entity Framework Core e PostgreSQL.

## Pré-requisitos

- Docker e Docker Compose
- .NET 9 SDK (opcional, para desenvolvimento local)

## Configuração

1. Clone o repositório.
2. Navegue para a pasta `backend`.
3. Copie o arquivo `.env.example` para `.env` e configure as variáveis de ambiente (senhas, etc.).

## Executando com Docker

### Quick Start

Para subir toda a aplicação (API + Banco):

```bash
docker compose up --build
```

A API estará disponível em `http://localhost:8080`.

### Build Manual da Imagem

Para construir apenas a imagem Docker:

```bash
docker build -f Nonfy.Api/Dockerfile -t backend-api:latest .
```

Para executar o container:

```bash
docker run -p 5000:80 backend-api:latest
```

### Aplicar Migrações

Após subir os containers:

```bash
docker compose exec api dotnet ef database update --startup-project /src/Nonfy.Api --project /src/Nonfy.Infrastructure
```

Ou com docker run:

```bash
docker run --rm --network host -e ASPNETCORE_ENVIRONMENT=Development -e ConnectionStrings__DefaultConnection="your-connection-string" backend-api:latest dotnet ef database update --startup-project /src/Nonfy.Api --project /src/Nonfy.Infrastructure
```

## Desenvolvimento Local

1. Instale o .NET 9 SDK.
2. Configure o PostgreSQL localmente ou use o Docker.
3. Restaure os pacotes: `dotnet restore`.
4. Execute as migrações: `dotnet ef database update` (no projeto Nonfy.Api).
5. Rode a API: `dotnet run` (no projeto Nonfy.Api).

## Tecnologias

- .NET 9
- Entity Framework Core 9
- PostgreSQL
- Docker
- BCrypt.Net-Next (password hashing)
- Minimal APIs

## API Endpoints

### Registrar Usuário

**POST** `/users`

Cria um novo usuário com email e senha.

**Request Body:**
```json
{
  "email": "usuario@example.com",
  "password": "SecurePass123@"
}
```

**Requisitos de Validação:**
- **Email**: Formato válido (ex: usuario@example.com)
- **Email**: Deve ser único no sistema (constraint de banco de dados)
- **Senha**: Mínimo 8 caracteres
- **Senha**: Deve conter maiúsculas, minúsculas, números e símbolos especiais

**Response (201 Created):**
```json
{
  "id": "d54bc052-b955-47dd-b578-667a948c4faa",
  "email": "usuario@example.com",
  "slug": "usuario"
}
```

**Error Responses:**

- **400 Bad Request** - Email inválido ou senha fraca
```json
{
  "error": "Email format is invalid. (Parameter 'email')"
}
```

- **409 Conflict** - Email já cadastrado
```json
{
  "error": "Email already exists."
}
```

- **500 Internal Server Error** - Erro não tratado

**Exemplo de Uso:**
```bash
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{"email": "novo@example.com", "password": "SecurePass123@"}'
```

**Notas de Segurança:**
- Senhas são hasheadas com BCrypt antes de serem armazenadas
- Response nunca expõe a senha ou hash
- O slug é gerado automaticamente a partir do prefixo do email

## Testando Endpoints

### Com curl

```bash
# Registrar novo usuário
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{"email": "teste@example.com", "password": "SecurePass123@"}'

# Com falha - email inválido
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{"email": "invalid", "password": "SecurePass123@"}'
```

### Com REST Client (VS Code)

Abra o arquivo `Nonfy.Api/test-users.http` no VS Code com a extensão REST Client instalada.
Os testes incluem:
- ✅ Registro válido
- ✅ Email inválido
- ✅ Senha fraca (muito curta)
- ✅ Senha sem maiúscula
- ✅ Senha sem símbolo especial
- ✅ Email duplicado

### OpenAPI (Swagger)

Em desenvolvimento, a documentação OpenAPI está disponível em:
```
http://localhost:8080/openapi/v1.json
```

## Architecture

O projeto segue a **Clean Architecture** com 4 camadas:

```
Nonfy.Api (Presentation Layer)
├── Contracts/        # Request/Response DTOs
│   ├── Requests/     # CreateUserRequest
│   └── Responses/    # CreateUserResponse
├── Services/         # Serviços de negócio
│   └── UserService   # Validação e registro de usuários
└── Program.cs        # Configuração e endpoints

Nonfy.Application (Application Layer)
└── [Camada de aplicação]

Nonfy.Domain (Domain Layer)
├── Entities/         # User (entidade de domínio)
└── [Lógica de domínio]

Nonfy.Infrastructure (Infrastructure Layer)
├── NonfyDbContext    # EF Core DbContext
└── Migrations/       # Migrações do banco
```

## Variáveis de Ambiente

Configure o arquivo `.env` com as seguintes variáveis:

```env
# Database
ConnectionStrings__DefaultConnection=Server=nonfy-db;Port=5432;Database=nonfy;User Id=postgres;Password=<sua-senha>;
ASPNETCORE_ENVIRONMENT=Development

# PostgreSQL
POSTGRES_PASSWORD=<sua-senha>
POSTGRES_USER=postgres
POSTGRES_DB=nonfy
```

Copie `.env.example` para `.env` e configure os valores reais.

## Troubleshooting

### Docker BuildKit Cache Issue
Se receber erro `failed to prepare extraction snapshot`:
```bash
docker builder prune
docker build -f Nonfy.Api/Dockerfile --no-cache -t backend-api:latest .
```

### Dockerfile não encontrado
O Dockerfile está em `Nonfy.Api/Dockerfile`. Use:
```bash
docker build -f Nonfy.Api/Dockerfile -t backend-api:latest .
```

### Porta já em uso
Se a porta 8080 ou 5000 estiver ocupada, mude em `docker-compose.yml`:
```yaml
ports:
  - "8081:80"  # Use 8081 em vez de 8080
```

## Segurança

- Senhas e secrets estão no `.env` (não versionado).
- Use senhas fortes e mude periodicamente.
- Nunca faça commit do `.env` ou secrets no Git.

## Contribuição

1. Faça um fork.
2. Crie uma branch para sua feature.
3. Commit suas mudanças.
4. Abra um PR.

## Licença

[Adicione licença aqui]