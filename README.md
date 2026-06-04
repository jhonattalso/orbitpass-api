# OrbitPass — Módulo de Ingressos e Pagamentos

> **FIAP — Global Solution 2026/1 | 2º Ano — Análise e Desenvolvimento de Sistemas**  
> Tema: O Espaço é a Nova Fronteira — Economia Espacial

API REST complementar do projeto **OrbitPass**, responsável pelo módulo de **Ingressos e Pagamentos** de uma plataforma de turismo espacial. Construída com ASP.NET Core, Clean Architecture, persistência Oracle via Entity Framework Core e autenticação JWT.

### Integrantes do Grupo
* **Jhonatta Lima Sandes de Oliveira** - RM: 560277
* **Rangel Bernadi Jordão** - RM: 560547
* **Lucas José Lima** - RM: 561160

**Turma:** 2TDSPA

---

## Por que Economia Espacial?

O turismo espacial é um mercado emergente e real — empresas como SpaceX, Blue Origin e Virgin Galactic já oferecem ou planejam oferecer experiências espaciais para civis. Porém, não existe ainda uma plataforma centralizada, acessível e inteligente para gerenciar reservas e auxiliar o cliente na escolha da melhor experiência.

O **OrbitPass** resolve isso conectando diretamente ao tema da Global Solution 2026/1: a **Economia Espacial**. Este repositório cobre o módulo de backend responsável pela compra, confirmação e cancelamento de ingressos para tours espaciais.

**ODS atendidos:**
- **ODS 8** — Trabalho Decente e Crescimento Econômico: novo modelo de negócio no setor espacial emergente
- **ODS 9** — Indústria, Inovação e Infraestrutura: uso de tecnologias avançadas (Clean Architecture, Oracle, JWT)
- **ODS 11** — Cidades e Comunidades Sustentáveis: plataforma digital conectando pessoas a experiências de alto valor tecnológico


---

## Stack Técnica

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 9.0 |
| Framework Web | ASP.NET Core 9 — Controllers |
| ORM | Entity Framework Core 9 + Oracle.EntityFrameworkCore 9 |
| Banco de Dados | Oracle Database (FIAP) |
| Autenticação | JWT Bearer (HMAC-SHA256) |
| Documentação | Swagger / Swashbuckle.AspNetCore |
| Testes | xUnit + FluentAssertions + Moq |
| Health Checks | AspNetCore.HealthChecks.Oracle |

---

## Arquitetura

O projeto segue os princípios de **Clean Architecture** com separação estrita em quatro camadas. As camadas internas nunca conhecem as externas.

```
┌─────────────────────────────────────────────┐
│              OrbitPass.Api                  │
│   Controllers · Middlewares · Extensions   │
└───────────────────┬─────────────────────────┘
                    │
        ┌───────────┴───────────┐
        ▼                       ▼
┌───────────────────┐  ┌────────────────────────┐
│OrbitPass.Applic.  │  │OrbitPass.Infrastructure│
│  Use Cases        │  │  EF Core · Oracle      │
│  Commands/Queries │  │  Repositories          │
│  DTOs · Handlers  │  │  DependencyInjection   │
└───────┬───────────┘  └────────────────────────┘
        │
        ▼
┌───────────────────┐
│  OrbitPass.Domain │
│  Entities · Enums │
│  Interfaces       │
│  Exceptions       │
└───────────────────┘
```

### Fluxo de uma Requisição

```
[Request HTTP]
│
▼
ExceptionHandlingMiddleware
│
▼
[Controller] → valida autenticação JWT
│
▼
[Handler] → executa regra de negócio
│
▼
[Repository] → persiste via EF Core
│
▼
[Oracle Database]
│
▼
[Response JSON]
```

---

## Modelagem do Banco de Dados

### Diagrama de Entidades
```
┌──────────────────────────────┐        ┌──────────────────────────────┐
│          INGRESSOS           │        │          PAGAMENTOS          │
├──────────────────────────────┤        ├──────────────────────────────┤
│ ID            RAW(16)   PK   │        │ ID            RAW(16)   PK   │
│ USUARIO_ID    RAW(16)        │◄───────│ INGRESSO_ID   RAW(16)   FK   │
│ DATA_TOUR_ID  RAW(16)        │        │ METODO        NUMBER(1)      │
│ CODIGO_UNICO  VARCHAR2(40)   │        │ STATUS        NUMBER(1)      │
│ STATUS        NUMBER(1)      │        │ DATA_PAGAMENTO TIMESTAMP     │
│ DATA_COMPRA   TIMESTAMP      │        │ VALOR         NUMBER(10,2)   │
│ VALOR_PAGO    NUMBER(10,2)   │        └──────────────────────────────┘
└──────────────────────────────┘
Relacionamento: 1 INGRESSO ──── 1 PAGAMENTO
```

### Tabela INGRESSOS

| Coluna | Tipo | Descrição |
|---|---|---|
| ID | RAW(16) | Chave primária (GUID) |
| USUARIO_ID | RAW(16) | ID do usuário comprador |
| DATA_TOUR_ID | RAW(16) | ID da data do tour espacial |
| CODIGO_UNICO | VARCHAR2(40) | Código único (ex: `OP-20260603-A1B2C3D4`) |
| STATUS | NUMBER(1) | 1=PendentePagamento · 2=Confirmado · 3=Cancelado |
| DATA_COMPRA | TIMESTAMP | Data e hora da compra (UTC) |
| VALOR_PAGO | NUMBER(10,2) | Valor pago pelo ingresso |

### Tabela PAGAMENTOS

| Coluna | Tipo | Descrição |
|---|---|---|
| ID | RAW(16) | Chave primária (GUID) |
| INGRESSO_ID | RAW(16) | FK para INGRESSOS — relacionamento 1:1 |
| METODO | NUMBER(1) | 1=CartaoCredito · 2=CartaoDebito · 3=Pix · 4=Transferencia |
| STATUS | NUMBER(1) | 1=Processando · 2=Aprovado · 3=Recusado |
| DATA_PAGAMENTO | TIMESTAMP | Data e hora do pagamento (UTC) |
| VALOR | NUMBER(10,2) | Valor do pagamento |

---

## Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Acesso ao Oracle Database (FIAP ou local)
- `dotnet-ef`: `dotnet tool install --global dotnet-ef --version 9.0.0`

---

## Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/OrbitPass.git
cd OrbitPass
```

### 2. Configure o ambiente local

Copie o arquivo de exemplo e preencha com seus dados reais:

```bash
cp src/OrbitPass.Api/appsettings.Development.example.json \
   src/OrbitPass.Api/appsettings.Development.json
```

Edite o `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "OracleDb": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=HOST:1521/SERVICE;"
  },
  "Jwt": {
    "SecretKey": "SuaChaveSecretaComMinimoVinteEQuatroCaracteres!"
  }
}
```

> ⚠️ O arquivo `appsettings.Development.json` está no `.gitignore` e **nunca é commitado**.
> Em produção e CI/CD, use as variáveis de ambiente `ORACLE_CONNECTION_STRING` e `JWT_SECRET_KEY`.

### 3. Aplique as Migrations

```bash
dotnet ef database update \
  --project src/OrbitPass.Infrastructure \
  --startup-project src/OrbitPass.Api
```

### 4. Execute a API

```bash
dotnet run --project src/OrbitPass.Api
```

Após iniciar:

| URL | Descrição |
|---|---|
| `http://localhost:5225/swagger` | Documentação interativa Swagger/OpenAPI |
| `http://localhost:5225/health` | Health Check da API e do banco Oracle |

---

## API Reference

### Autenticação

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/Auth/token` | ❌ | Gera token JWT para uso nos demais endpoints |

**Credenciais de teste:**
```json
{
  "email": "teste@orbitpass.com",
  "senha": "Senha@123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Ingressos

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/ingressos` | ✅ JWT | Compra um ingresso e processa o pagamento |
| `GET` | `/api/ingressos/usuario/{usuarioId}` | ✅ JWT | Lista todos os ingressos do usuário |
| `DELETE` | `/api/ingressos/{ingressoId}/usuario/{usuarioId}` | ✅ JWT | Cancela um ingresso |

### Pagamentos

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/pagamentos` | ✅ JWT | Processa o pagamento de um ingresso existente |

### Monitoramento

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/health` | Status geral da API e conectividade com Oracle |

---

## Como Usar (Fluxo Completo)

### 1. Gerar o token JWT

```bash
POST /api/Auth/token
Content-Type: application/json

{
  "email": "teste@orbitpass.com",
  "senha": "Senha@123"
}
```

### 2. Autorizar no Swagger

Clique em **Authorize** e informe:
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

### 3. Comprar um ingresso

```bash
POST /api/ingressos
Authorization: Bearer {token}
Content-Type: application/json

{
  "usuarioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "dataTourId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "valorPago": 1500.00,
  "metodo": 3
}
```

**Valores válidos para `metodo`:** `1` CartaoCredito · `2` CartaoDebito · `3` Pix · `4` Transferencia

**Resposta:**
```json
{
  "ingressoId": "a1b2c3d4-...",
  "codigoUnico": "OP-20260603-A1B2C3D4",
  "statusIngresso": 2,
  "pagamentoId": "e5f6g7h8-...",
  "statusPagamento": 2
}
```

### 4. Listar ingressos do usuário

```bash
GET /api/ingressos/usuario/3fa85f64-5717-4562-b3fc-2c963f66afa6
Authorization: Bearer {token}
```

### 5. Cancelar um ingresso

```bash
DELETE /api/ingressos/{ingressoId}/usuario/{usuarioId}
Authorization: Bearer {token}
```

Retorna `204 No Content` em caso de sucesso.

---

## Tratamento de Erros

O middleware global captura todas as exceções e retorna respostas padronizadas:

| Situação | HTTP Status | Tipo |
|---|---|---|
| Regra de negócio violada (ex: cancelar ingresso já cancelado) | `422` | `Regra de negócio` |
| Recurso não encontrado | `404` | `Recurso não encontrado` |
| Token JWT ausente ou inválido | `401` | — |
| Erro inesperado | `500` | `Erro interno` |

**Exemplo de resposta de erro:**
```json
{
  "status": 422,
  "tipo": "Regra de negócio",
  "mensagem": "Ingresso já está cancelado.",
  "timestamp": "2026-06-03T12:00:00Z"
}
```

---

## Testes Automatizados

Execute a suíte completa com:

```bash
dotnet test
```

Para saída detalhada:

```bash
dotnet test --logger "console;verbosity=detailed"
```

O projeto possui **9 testes automatizados** organizados em três classes, todos seguindo o padrão **AAA (Arrange, Act, Assert)**:

### ComprarIngressoHandlerTests (3 testes)

| Teste | Cenário |
|---|---|
| `Handle_DeveCriarIngresso_QuandoDadosValidos` | Fluxo feliz — ingresso confirmado e pagamento aprovado |
| `Handle_DeveLancarDomainException_QuandoValorZero` | Valor inválido lança exceção de domínio |
| `Handle_DeveGerarCodigoUnico_ParaCadaIngresso` | Dois ingressos sempre geram códigos distintos |

### CancelarIngressoHandlerTests (3 testes)

| Teste | Cenário |
|---|---|
| `Handle_DeveCancelarIngresso_QuandoUsuarioCorreto` | Usuário dono do ingresso cancela com sucesso |
| `Handle_DeveLancarDomainException_QuandoUsuarioDiferente` | Usuário sem permissão lança exceção de domínio |
| `Handle_DeveLancarKeyNotFound_QuandoIngressoNaoExiste` | Ingresso inexistente lança KeyNotFoundException |

### ProcessarPagamentoHandlerTests (3 testes)

| Teste | Cenário |
|---|---|
| `Handle_DeveProcessarPagamento_QuandoIngressoValido` | Pagamento processado e aprovado com sucesso |
| `Handle_DeveLancarDomainException_QuandoPagamentoDuplicado` | Segundo pagamento no mesmo ingresso lança exceção |

---

## Estrutura do Repositório
```
OrbitPass/
├── src/
│   ├── OrbitPass.Domain/
│   │   ├── Entities/          # Ingresso.cs · Pagamento.cs
│   │   ├── Enums/             # StatusIngresso · StatusPagamento · MetodoPagamento
│   │   ├── Exceptions/        # DomainException.cs
│   │   └── Interfaces/        # IIngressoRepository · IPagamentoRepository
│   │
│   ├── OrbitPass.Application/
│   │   ├── DTOs/              # IngressoDto · PagamentoDto
│   │   └── UseCases/
│   │       ├── Ingressos/     # ComprarIngresso · CancelarIngresso · ListarIngressos
│   │       └── Pagamentos/    # ProcessarPagamento
│   │
│   ├── OrbitPass.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── Context/       # AppDbContext.cs
│   │   │   ├── Mappings/      # IngressoMapping · PagamentoMapping
│   │   │   └── Repositories/  # IngressoRepository · PagamentoRepository
│   │   └── DependencyInjection.cs
│   │
│   └── OrbitPass.Api/
│       ├── Controllers/       # AuthController · IngressosController · PagamentosController
│       ├── Extensions/        # JwtExtensions · SwaggerExtensions · HealthCheckExtensions
│       ├── Middlewares/       # ExceptionHandlingMiddleware.cs
│       └── Program.cs
│
├── tests/
│   └── OrbitPass.Tests/
│       └── UseCases/          # 9 testes xUnit (padrão AAA)
│
├── scripts/
│   └── script-bd.sql          # DDL Oracle gerado pelas Migrations (--idempotent)
│
├── dockerfiles/               # Dockerfiles para containerização (DevOps)
├── .gitignore
└── README.md
```

---

## Segurança

- Nenhuma credencial, senha ou chave secreta está presente em arquivos commitados
- `appsettings.Development.json` está no `.gitignore`
- Em produção, as variáveis `ORACLE_CONNECTION_STRING` e `JWT_SECRET_KEY` sobrescrevem qualquer valor do `appsettings.json`
- Todas as rotas exceto `POST /api/Auth/token` exigem JWT válido (`[Authorize]`)
- O arquivo `appsettings.Development.example.json` documenta a estrutura esperada sem expor valores reais

---

*FIAP — Global Solution 2026/1 — 2º Ano ADS*
