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

Para subir toda a aplicação (API + Banco):

```bash
docker compose up --build
```

A API estará disponível em `http://localhost:8080`.

### Aplicar Migrações

Após subir os containers:

```bash
docker compose exec nonfy-api dotnet ef database update --startup-project /src/Nonfy.Api --project /src/Nonfy.Infrastructure
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

## Segurança

- Senhas e secrets estão no `.env` (não versionado).
- Use senhas fortes e mude periodicamente.

## Contribuição

1. Faça um fork.
2. Crie uma branch para sua feature.
3. Commit suas mudanças.
4. Abra um PR.

## Licença

[Adicione licença aqui]