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
docker compose exec nonfy-api dotnet ef database update --startup-project /src/Nonfy.Api --project /src/Nonfy.Infrastructure
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

## Estrutura do Projeto

- **Nonfy.Api**: API Web com ASP.NET Core.
- **Nonfy.Domain**: Entidades e lógica de domínio.
- **Nonfy.Application**: Casos de uso e serviços.
- **Nonfy.Infrastructure**: Acesso a dados com EF Core.

## Tecnologias

- .NET 9
- Entity Framework Core 9
- PostgreSQL
- Docker

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