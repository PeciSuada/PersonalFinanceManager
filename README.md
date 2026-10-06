# PersonalFinanceManager

A REST API for tracking personal income and expenses. Transactions are grouped into categories, and the API can return a summary of total income, total expenses and balance for any period.

## Tech stack

- .NET 10, ASP.NET Core Web API (controllers)
- Entity Framework Core with SQL Server
- Scalar for interactive API documentation (OpenAPI)
- xUnit with SQLite in-memory database for tests

## Project structure

```
src/Api            The Web API
  Controllers/     HTTP endpoints
  Services/        Business logic and database access
  Data/            AppDbContext
  Models/          Category, Transaction, Summary
  DTOs/            Request models with validation
  Migrations/      EF Core migrations
tests/Api.Tests    xUnit tests for the services
```

## Getting started

### Prerequisites

- .NET 10 SDK
- SQL Server (the project is configured for a local SQL Server Express instance)
- EF Core tools: `dotnet tool install --global dotnet-ef`

### Setup

1. Check the connection string in `src/Api/appsettings.json` and change it if your SQL Server instance is different:

```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PersonalFinanceManager;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
```

2. Create the database by applying the migrations:

```
   dotnet ef database update --project src/Api
```

3. Run the API:

```
   dotnet run --project src/Api
```

The API listens on `http://localhost:5205`.

## API documentation

In the Development environment, interactive documentation is available at:

```
http://localhost:5205/scalar/v1
```

You can send requests to every endpoint directly from the browser. The raw OpenAPI document is served at `/openapi/v1.json`.

## Endpoints

### Categories

| Method | Path | Description | Responses |
|--------|------|-------------|-----------|
| GET | `/api/categories` | List all categories | 200 |
| GET | `/api/categories/{id}` | Get one category | 200, 404 |
| POST | `/api/categories` | Create a category | 201, 400 |
| PUT | `/api/categories/{id}` | Rename a category | 204, 400, 404 |
| DELETE | `/api/categories/{id}` | Delete a category | 204, 404, 409 |

A category cannot be deleted while it has transactions. The API returns `409 Conflict` instead.

### Transactions

| Method | Path | Description | Responses |
|--------|------|-------------|-----------|
| GET | `/api/transactions` | List transactions (with optional filters) | 200 |
| GET | `/api/transactions/{id}` | Get one transaction, including its category | 200, 404 |
| POST | `/api/transactions` | Create a transaction | 201, 400 |
| PUT | `/api/transactions/{id}` | Update a transaction | 204, 400, 404 |
| DELETE | `/api/transactions/{id}` | Delete a transaction | 204, 404 |
| GET | `/api/transactions/summary` | Total income, total expenses and balance | 200 |

**Filters** for `GET /api/transactions` and `GET /api/transactions/summary` (all optional):

| Parameter | Description |
|-----------|-------------|
| `categoryId` | Only transactions of this category (list endpoint only) |
| `from` | Transactions on or after this date |
| `to` | Transactions up to and including this whole day |

Example: `GET /api/transactions?categoryId=1&from=2026-10-01&to=2026-10-31`

**Request body** for `POST` and `PUT`:

```json
{
  "amount": 20.00,
  "description": "Coffee",
  "date": "2026-10-03T10:00:00",
  "type": 1,
  "categoryId": 1
}
```

`type` is `0` for income and `1` for expense. The category must exist, otherwise the API returns `400 Bad Request`.

**Summary response:**

```json
{
  "totalExpense": 100.50,
  "totalIncome": 1200.00,
  "balance": 1099.50
}
```

### Validation

- Category name is required.
- Transaction amount must be between 0.05 and 1,000,000.
- Transaction description is required.
- Invalid requests return `400 Bad Request` with a description of the problem.

## Tests

```
dotnet test
```

The tests cover `CategoryService` and `TransactionService` (creating, reading, filtering, updating, deleting, summary calculation and the rule that blocks deleting a category with transactions). Each test runs against a fresh SQLite in-memory database, so no SQL Server is needed to run them.