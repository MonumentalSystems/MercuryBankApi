# MercuryBankApi

[![CI](https://github.com/MonumentalSystems/MercuryBankApi/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/MonumentalSystems/MercuryBankApi/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/MonumentalSystems.MercuryBankApi.svg)](https://www.nuget.org/packages/MonumentalSystems.MercuryBankApi)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MonumentalSystems.MercuryBankApi.svg)](https://www.nuget.org/packages/MonumentalSystems.MercuryBankApi)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/MonumentalSystems/MercuryBankApi/blob/master/LICENSE)

Strongly-typed C# client for the [Mercury Bank API](https://docs.mercury.com/reference), auto-generated from OpenAPI spec via NSwag.

## Installation

```bash
dotnet add package MonumentalSystems.MercuryBankApi
```

## Quick Start

```csharp
// In Program.cs or Startup
builder.Services.AddMercuryBankApi(builder.Configuration);
```

```json
// appsettings.json (or user-secrets for the token)
{
  "Mercury": {
    "ApiToken": "secret-token:mercury_production_...",
    "BaseUrl": "https://api.mercury.com/api/v1"
  }
}
```

```csharp
// Inject and use
public class MyService(IMercuryBankClient mercury)
{
    public async Task DoWork()
    {
        var accounts = await mercury.GetAccountsAsync();
        var transactions = await mercury.GetTransactionsAsync(
            DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)),
            DateOnly.FromDateTime(DateTime.UtcNow));
    }
}
```

## API Surface

| Method | Description |
|--------|-------------|
| `GetAccountsAsync()` | List all Mercury accounts |
| `GetAccountAsync(Guid)` | Get a single account by ID |
| `GetTransactionsAsync(DateOnly, DateOnly)` | List transactions across all accounts |
| `GetAccountTransactionsAsync(Guid, DateOnly, DateOnly)` | List transactions for a specific account |
| `GetTransactionAsync(Guid)` | Get a single transaction by ID |

## Configuration Options

| Option | Default | Description |
|--------|---------|-------------|
| `BaseUrl` | `https://api.mercury.com/api/v1` | Mercury API base URL |
| `ApiToken` | *(empty)* | Bearer token from Mercury dashboard |

## Regenerating the Client

The client is generated from Mercury's OpenAPI spec using NSwag:

```bash
# 1. Scrape latest OpenAPI spec from Mercury docs
cd openapi && node scrape-mercury-openapi.js

# 2. Regenerate C# client
nswag openapi2csclient \
  /input:mercury-openapi.json \
  /output:../src/MercuryBankApi/Generated/MercuryApiClient.cs \
  /namespace:MercuryBankApi.Generated \
  /className:MercuryApiClient \
  /generateClientInterfaces:true \
  /generateDtoTypes:true \
  /injectHttpClient:true \
  /useBaseUrl:false \
  /jsonLibrary:SystemTextJson
```

## License

MIT
