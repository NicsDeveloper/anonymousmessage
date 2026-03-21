# Contrato da API - Nonfy Backend

## Endpoint: Registrar Usuário

### 📌 Informações Gerais

| Propriedade | Valor |
|-------------|-------|
| **Método HTTP** | `POST` |
| **Rota** | `/users` |
| **Versão** | 1.0 |
| **Autenticação** | Não requerida |
| **Content-Type** | `application/json` |
| **Versão do .NET** | 9.0 |

---

## 📥 Request

### URL
```
POST http://localhost:8080/users
```

### Headers
```
Content-Type: application/json
```

### Body

**Type:** `application/json`

**Schema:**
```json
{
  "email": "string",
  "password": "string"
}
```

**Parâmetros:**

| Campo | Tipo | Obrigatório | Tamanho | Descrição |
|-------|------|-------------|--------|-----------|
| `email` | string | ✅ Sim | 1-255 | Email do usuário. Deve ser um email válido e único |
| `password` | string | ✅ Sim | 8+ | Senha do usuário. Deve conter maiúsculas, minúsculas, números e símbolos |

### Regras de Validação

#### Email
- ✅ Deve ser um email válido (formato correto)
- ✅ Deve ser único no banco de dados
- ❌ Não pode ser null ou vazio

**Exemplo válido:** `usuario@example.com`
**Exemplo inválido:** `usuario@`, `@example.com`, `usuario`

#### Senha
- ✅ Mínimo de 8 caracteres
- ✅ Deve conter PELO MENOS 1 maiúscula (A-Z)
- ✅ Deve conter PELO MENOS 1 minúscula (a-z)
- ✅ Deve conter PELO MENOS 1 número (0-9)
- ✅ Deve conter PELO MENOS 1 símbolo especial: `!@#$%^&*()_+-=[]{}';:",.<>?/\|~\``
- ❌ Não pode ser null ou vazio

**Exemplo válido:** `SecurePass123@`, `MyPassword1!`, `Test@Pass99`
**Exemplo inválido:**
- `short1@` (menos de 8 caracteres)
- `password123@` (sem maiúsculas)
- `PASSWORD123@` (sem minúsculas)
- `Password@` (sem números)
- `Password123` (sem símbolos)

---

### Exemplos de Request

#### ✅ Sucesso
```bash
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@example.com",
    "password": "SecurePass123@"
  }'
```

#### ❌ Email Inválido
```bash
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao.example.com",
    "password": "SecurePass123@"
  }'
```

#### ❌ Senha Fraca
```bash
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@example.com",
    "password": "weak"
  }'
```

---

## 📤 Response

### Success Response (201 Created)

**Status Code:** `201 Created`

**Headers:**
```
Content-Type: application/json
Location: /users/{id}
```

**Body Schema:**
```json
{
  "id": "uuid",
  "email": "string",
  "slug": "string"
}
```

**Parâmetros da Resposta:**

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `id` | UUID | Identificador único do usuário gerado automaticamente (v4) |
| `email` | string | Email cadastrado |
| `slug` | string | Identificador amigável gerado a partir do prefixo do email |

**Exemplo:**
```json
{
  "id": "d54bc052-b955-47dd-b578-667a948c4faa",
  "email": "joao@example.com",
  "slug": "joao"
}
```

**Notas de Segurança:**
- ⚠️ A resposta **NUNCA** contém a senha (`password`)
- ⚠️ A resposta **NUNCA** contém o hash da senha (`passwordHash`)
- ✅ Apenas dados públicos são retornados

---

## ❌ Error Responses

### 400 Bad Request

**Casos:**
- Email vazio ou null
- Senha vazia ou null
- Email em formato inválido
- Senha menor que 8 caracteres
- Senha sem maiúscula, minúscula, número ou símbolo

**Exemplo:**
```json
{
  "error": "Email format is invalid. (Parameter 'email')"
}
```

Ou:

```json
{
  "error": "Password must be at least 8 characters long. (Parameter 'password')"
}
```

Ou:

```json
{
  "error": "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*()_+-=[]{}';:\",.<>?/\\|`~). (Parameter 'password')"
}
```

---

### 409 Conflict

**Caso:** Email já existe no banco de dados

**Status Code:** `409`

**Body:**
```json
{
  "error": "Email already exists."
}
```

---

### 500 Internal Server Error

**Caso:** Erro não tratado no servidor

**Status Code:** `500`

**Body:**
```json
{
  "error": "Internal server error"
}
```

---

## 🔄 Fluxo de Processamento

```
1. Cliente envia requisição POST /users
   ↓
2. Validação de entrada (email e password não vazios)
   ├─ Se vazio → Retorna 400 Bad Request
   ↓
3. Validação de email (formato)
   ├─ Se inválido → Retorna 400 Bad Request
   ↓
4. Validação de senha (comprimento e complexidade)
   ├─ Se inválido → Retorna 400 Bad Request
   ↓
5. Hash da senha com BCrypt
   ↓
6. Geração de slug a partir do prefixo do email
   ↓
7. Criação da entidade User
   ├─ Validação de negócio na entidade
   ├─ Se falhar → Retorna 400 Bad Request
   ↓
8. Persistência no banco de dados
   ├─ Se email duplicado (constraint violation) → Retorna 409 Conflict
   ├─ Se outro erro de banco → Retorna 500 Internal Server Error
   ↓
9. Sucesso → Retorna 201 Created com dados públicos
```

---

## 🔐 Segurança

### Implementado
- ✅ **Hashing de Senha:** BCrypt com salt automático
- ✅ **Validação Rigorosa:** Email válido + Senha forte
- ✅ **Constraint Único:** Email único no banco de dados
- ✅ **Resposta Segura:** Nunca expõe password ou hash
- ✅ **Validação em Camadas:** API + Domínio

### Não Implementado (Futuro)
- ⚠️ Rate Limiting
- ⚠️ CORS
- ⚠️ Autenticação JWT
- ⚠️ HTTPS obrigatório em produção
- ⚠️ Logging de tentativas de registro

---

## 📊 Detalhes Técnicos

### Geração de Slug

```
Email: usuario@example.com
       ↓
Prefixo: usuario
       ↓
Slug: usuario (em minúsculas)
```

O slug é gerado pegando a parte antes do `@` do email e convertendo para minúsculas.

### Hashing de Senha

```
Senha: SecurePass123@
       ↓
BCrypt Hash (com salt): $2a$11$...
       ↓
Persistida no banco
```

Usa BCrypt.Net-Next com custo padrão (11 rounds).

### User Entity

```csharp
public class User
{
    public Guid Id { get; private set; }              // UUID gerado
    public string BusinessName { get; private set; }  // Email (campo)
    public string Email { get; private set; }         // Email único
    public string PasswordHash { get; private set; }  // BCrypt hash
    public string Slug { get; private set; }          // slug único
    public DateTime CreatedAt { get; private set; }   // Timestamp UTC
}
```

---

## 📝 DTOs do Endpoint

### CreateUserRequest
```csharp
public record CreateUserRequest(
    string Email,
    string Password
);
```

### CreateUserResponse
```csharp
public record CreateUserResponse(
    Guid Id,
    string Email,
    string Slug
);
```

---

## 🧪 Casos de Teste

| # | Entrada | Esperado | Status |
|---|---------|----------|--------|
| 1 | Email válido + Senha forte | Usuário criado | 201 ✅ |
| 2 | Email inválido | Erro de validação | 400 ✅ |
| 3 | Senha < 8 caracteres | Erro de validação | 400 ✅ |
| 4 | Senha sem maiúscula | Erro de validação | 400 ✅ |
| 5 | Senha sem minúscula | Erro de validação | 400 ✅ |
| 6 | Senha sem número | Erro de validação | 400 ✅ |
| 7 | Senha sem símbolo | Erro de validação | 400 ✅ |
| 8 | Email duplicado | Erro de constraint | 409 ✅ |
| 9 | Email vazio | Erro de validação | 400 ✅ |
| 10 | Senha vazia | Erro de validação | 400 ✅ |

---

## 📚 Documentação OpenAPI

A documentação OpenAPI completa está disponível em:
```
GET http://localhost:8080/openapi/v1.json
```

Em desenvolvimento, visualize em:
```
http://localhost:8080/openapi/ui
```

---

## 🔄 Versionamento

**Versão Atual:** 1.0
**Data de Criação:** 21/03/2026
**Última Atualização:** 21/03/2026

---

## 📞 Contato / Suporte

Para dúvidas sobre este contrato ou para reportar bugs:
- Abra uma issue no repositório
- Entre em contato com o time de desenvolvimento
