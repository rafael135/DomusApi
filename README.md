# Domus — Controle de Gastos Residenciais

API REST para gerenciamento de finanças domésticas: cadastro de pessoas, categorias e transações, com relatórios de totais por pessoa e por categoria.

> Frontend: [rafael135/DomusFrontend](https://github.com/rafael135/DomusFrontend)

## Stack

.NET 10 · ASP.NET Core Minimal API · EF Core 10 · SQL Server 2025 · MediatR · Scalar (docs) · xUnit · Testcontainers

## Arquitetura

Clean Architecture + DDD + CQRS em fatias verticais por funcionalidade:

```
Domus.Core            ← domínio (entidades, regras de negócio)
Domus.Infrastructure  ← persistência (EF Core, migrations)
Domus.API             ← endpoints Minimal API + handlers MediatR
```

Erros retornam no formato **RFC 7807 Problem Details**.

---

## Como executar

**Pré-requisitos:** [.NET 10 SDK](https://dotnet.microsoft.com/download) e [Docker](https://www.docker.com/)

```bash
# 1. Subir o banco (SQL Server 2025)
docker-compose up -d

# 2. Aplicar migrations
.\update-db.ps1          # Windows
./update-db.sh           # Linux / macOS

# 3. Rodar a API
.\run.ps1                # Windows
./run.linux.sh           # Linux / macOS
```

| | URL |
|---|---|
| HTTP | `http://localhost:5035` |
| HTTPS | `https://localhost:7155` |
| Docs (dev) | `https://localhost:7155/scalar/v1` |

---

## Endpoints

Respostas paginadas aceitam `pageNumber` e `pageSize` como query params.

### Pessoas `/api/users`

| Método | Rota | |
|--------|------|-|
| `GET` | `/api/users` | lista (filtro: `searchTerm`) |
| `POST` | `/api/users` | cria |
| `PUT` | `/api/users/{userId}` | atualiza |
| `DELETE` | `/api/users/{userId}` | remove (cascateia transações) |

```json
{ "name": "João Silva", "age": 30 }
```

### Categorias `/api/categories`

| Método | Rota | |
|--------|------|-|
| `GET` | `/api/categories` | lista (filtro: `finality`) |
| `POST` | `/api/categories` | cria |

```json
{ "description": "Alimentação", "finality": 1 }
```

`finality`: `1` Despesa · `2` Receita · `3` Ambas

### Transações `/api/transactions`

| Método | Rota | |
|--------|------|-|
| `GET` | `/api/transactions` | lista (filtros: `userId`, `categoryId`) |
| `POST` | `/api/transactions` | cria |

```json
{ "description": "Supermercado", "value": 350.00, "type": 2, "categoryId": "...", "userId": "..." }
```

`type`: `1` Receita · `2` Despesa

> **Regras:** menores de 18 só podem registrar despesas; a finalidade da categoria deve ser compatível com o tipo da transação.

### Relatórios `/api/reports`

| Rota | |
|------|-|
| `/api/reports/totals-by-person` | receitas, despesas e saldo por pessoa + total geral |
| `/api/reports/totals-by-category` | receitas, despesas e saldo por categoria + total geral |

---

## Testes

```bash
dotnet test tests/Domus.Core.Tests           # unitários (domínio puro)
dotnet test tests/Domus.Integration.Tests    # integração (requer Docker)
```

Os testes de integração sobem um container SQL Server via Testcontainers e executam requisições HTTP reais contra a API.
