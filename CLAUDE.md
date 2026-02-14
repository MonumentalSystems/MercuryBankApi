# MercuryBankApi

## Build & Test Commands
- Build: `dotnet build`
- Test all: `dotnet test`
- Test single: `dotnet test --filter "FullyQualifiedName~MercuryBankClientTests.GetAccountsAsync_ReturnsAccounts"`
- Pack: `dotnet pack src/MercuryBankApi -c Release -o ./artifacts`
- Regenerate client: `cd openapi && nswag openapi2csclient /input:mercury-openapi.json /output:../src/MercuryBankApi/Generated/MercuryApiClient.cs /namespace:MercuryBankApi.Generated /className:MercuryApiClient /generateClientInterfaces:true /generateDtoTypes:true /injectHttpClient:true /useBaseUrl:false /jsonLibrary:SystemTextJson`
- Update OpenAPI spec: `cd openapi && node scrape-mercury-openapi.js`

## Architecture
- **MercuryBankApi** (`src/MercuryBankApi`): NuGet library package. Public API surface is `IMercuryBankClient`, `MercuryBankOptions`, and `ServiceCollectionExtensions.AddMercuryBankApi()`.
- **Generated** (`src/MercuryBankApi/Generated`): NSwag auto-generated client from Mercury OpenAPI spec. Do NOT edit manually — regenerate with NSwag.
- **OpenAPI** (`openapi/`): Mercury OpenAPI spec (`mercury-openapi.json`) and scraper (`scrape-mercury-openapi.js`) to rebuild it from Mercury docs.
- **Tests** (`tests/MercuryBankApi.Tests`): xUnit tests with FluentAssertions and NSubstitute.

## Code Style
- Target: .NET 10, C# latest, nullable enabled, implicit usings, warnings as errors
- Central package management via `Directory.Packages.props`
- XML doc comments required on all public types and members
- Generated code in `Generated/` namespace — never edit, always regenerate
- `IMercuryBankClient` is the public interface; `IMercuryApiClient` is the generated low-level interface
- Use `MercuryBankOptions` for configuration, bind via `IConfiguration` or `Action<MercuryBankOptions>`

## Versioning
- SemVer 2.0: `MAJOR.MINOR.PATCH`
- Version derived from git tags via MinVer (e.g., `v0.1.0` tag → `0.1.0`)
- NuGet publish triggered by pushing a `v*` tag (e.g., `v0.1.0`)
- Breaking API changes = bump MAJOR, new features = bump MINOR, fixes = bump PATCH

## NuGet
- Package ID: `MonumentalSystems.MercuryBankApi`
- GitHub Packages: auto-published on tag push
- NuGet.org: published if `NUGET_API_KEY` secret is set
