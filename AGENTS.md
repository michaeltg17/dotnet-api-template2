# Template

.NET 10 layered ASP.NET Core API template with a clean architecture: Api, ApiClient, Application, Core, CrossCutting, Domain, and Persistence projects.

## Architecture

- **Api** — ASP.NET Core minimal API with endpoints organized by resource
- **Application** — Business logic, services, exceptions, and request/response models
- **Domain** — Domain entities (EF Core models) and validators
- **Persistence** — EF Core DbContext, configurations, and data access
- **Persistence.Migrations** — dbup-based SQL migration runner
- **Core** — Builder pattern and helpers
- **CrossCutting** — Shared concerns: logging extensions, settings, and DI configurators
- **ApiClient** — HTTP client library with endpoints, extensions, and exceptions

## Structure

```
├── .dockerignore
├── .editorconfig
├── .gitattributes
├── .gitignore
├── AGENTS.md
├── ci-docker.sh                    # CI docker run script
├── ci.sh                           # CI entrypoint script
├── Directory.Build.props           # shared props: net10.0, nullable, implicit usings
├── Directory.Packages.props        # central package versions
├── docker-compose.yml              # sqlserver, migrator, api services
├── Dockerfile                      # multi-stage: SDK build → runtime (aspnet:10.0)
├── Dockerfile.ci                   # CI runtime image with test dependencies
├── Dockerfile.migrations           # multi-stage: SDK build → runtime for migrator
├── README.md
├── Template.slnx
├── .github/workflows/              # GH Actions
│   └── ci.yml
├── src/
│   ├── Api/                        # ASP.NET Core minimal API
│   │   ├── Api.csproj
│   │   ├── Program.cs              # entrypoint (delegates to Startup.Run)
│   │   ├── Startup.cs              # DI, Serilog, endpoint registration
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Endpoints/
│   │   │   ├── Test/
│   │   │   │   ├── GetOkEndpoint.cs
│   │   │   │   ├── PostEndpoint.cs
│   │   │   │   └── ThrowInternalServerErrorEndpoint.cs
│   │   │   └── Products/
│   │   │       ├── CreateProductEndpoint.cs
│   │   │       ├── GetProductEndpoint.cs
│   │   │       ├── GetAllProductsEndpoint.cs
│   │   │       ├── UpdateProductEndpoint.cs
│   │   │       └── DeleteProductsEndpoint.cs
│   │   ├── Extensions/
│   │   │   ├── EndpointExtensions.cs
│   │   │   ├── ExceptionHandlerExtensions.cs
│   │   │   └── ValidationFailureExtensions.cs
│   │   ├── Models/
│   │   │   └── Requests/
│   │   │       └── PostRequest.cs
│   │   └── Properties/
│   │       └── launchSettings.json
│   ├── ApiClient/                  # HTTP client library
│   │   ├── ApiClient.csproj
│   │   ├── ApiClient.cs
│   │   ├── Converters/
│   │   │   └── NestedObjectConverter.cs
│   │   ├── Endpoints/
│   │   │   └── TestEndpoints.cs
│   │   ├── Exceptions/
│   │   │   ├── ApiClientException.cs
│   │   │   └── ApiException.cs
│   │   └── Extensions/
│   │       ├── HttpResponseMessageExtensions.cs
│   │       └── ProblemDetailsExtensions.cs
│   ├── Application/                # business logic
│   │   ├── Application.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── Exceptions/
│   │   │   ├── TemplateException.cs       # base exception
│   │   │   ├── NotFoundException.cs
│   │   │   ├── NotFoundException(T).cs
│   │   │   ├── NotAllFoundException.cs
│   │   │   └── NotAllFoundException(T).cs
│   │   ├── Models/
│   │   │   ├── Requests/
│   │   │   │   ├── CreateProductRequest.cs
│   │   │   │   ├── UpdateProductRequest.cs
│   │   │   │   └── DeleteProductsRequest.cs
│   │   │   └── Responses/
│   │   │       └── DeleteProductsResponse.cs
│   │   └── Services/
│   │       └── ProductService.cs
│   ├── Core/                       # builder pattern and helpers
│   │   ├── Core.csproj
│   │   ├── Builders/
│   │   │   ├── Builder.cs
│   │   │   ├── BuilderWithInstance.cs
│   │   │   ├── BuilderWithValues.cs
│   │   │   └── IBuilder.cs
│   │   └── Extensions/
│   │       └── TypeExtensions.cs       # helper for type names
│   ├── CrossCutting/               # shared concerns
│   │   ├── CrossCutting.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── Logging/
│   │   │   └── ILoggerExtensions.cs    # source-generated log messages
│   │   └── Settings/
│   │       ├── ITemplateSettings.cs
    │   │       ├── TemplateSettings.cs     # POCO bound from config
    │   │       └── TemplateSettingsValidator.cs # IValidateOptions for settings
│   ├── Domain/                     # domain entities
│   │   ├── Domain.csproj
│   │   ├── Models/
│   │   │   ├── Entity.cs
│   │   │   └── Product.cs
│   │   └── Validators/
│   │       └── ProductValidator.cs
│   ├── Persistence/                # EF Core data access
│   │   ├── Persistence.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── AppDbContext.cs
│   │   └── Configurations/
│   │       ├── EntityConfiguration.cs
│   │       └── ProductConfiguration.cs
│   └── Persistence.Migrations/     # dbup SQL migration runner
│       ├── Persistence.Migrations.csproj
│       ├── Program.cs
│       ├── Migrator.cs
│       ├── Extensions/
│       │   └── DatabaseUpgradeResultExtensions.cs
│       └── Scripts/
│           └── 0001_Initial.sql
└── tests/
    ├── Core.Testing/               # shared test utilities (builders, validators)
    │   ├── Core.Testing.csproj
    │   ├── Builders/
    │   │   ├── ProductBuilder.cs
    │   │   ├── CreateProductRequestBuilder.cs
    │   │   ├── UpdateProductRequestBuilder.cs
    │   │   ├── DeleteProductsRequestBuilder.cs
    │   │   ├── DeleteProductsResponseBuilder.cs
    │   │   └── ProblemDetailsBuilder.cs
    │   ├── Extensions/
    │   │   ├── HttpResponseMessageExtensions.cs
    │   │   ├── ProblemDetailsExtensions.cs
    │   │   └── LogEventPropertyAssertionExtensions.cs
    │   └── Validators/
    │       ├── ExceptionValidator.cs
    │       ├── ProblemDetailsValidator.cs
    │       └── TraceIdValidator.cs
    ├── UnitTests/                  # isolated unit tests
    │   ├── UnitTests.csproj
    │   ├── Core/
    │   │   └── TypeExtensionsTests.cs
    │   ├── Core/Testing/Validators/
    │   │   ├── ExceptionValidatorTests.cs
    │   │   └── TraceIdValidatorTests.cs
    │   └── Domain/Validators/
    │       └── ProductValidatorTests.cs
    ├── IntegrationTests/           # WebApplicationFactory + Testcontainers.MsSql
    │   ├── IntegrationTests.csproj
    │   ├── BeforeAfterTestConfiguration.cs
    │   ├── Startup.cs
    │   ├── Test.cs
    │   ├── IntegrationTestsException.cs
    │   ├── TestStartupFilter.cs
    │   ├── xunit.runner.json
    │   ├── Settings/
    │   │   ├── ITestSettings.cs
    │   │   └── TestSettings.cs
    │   ├── Collections/
    │   │   ├── DevelopmentApiCollection.cs
    │   │   └── ProductionApiCollection.cs
    │   ├── Fixtures/
    │   │   ├── WebApplicationFactoryFixture.cs
    │   │   ├── DevelopmentWebApplicationFactoryFixture.cs
    │   │   └── ProductionWebApplicationFactoryFixture.cs
    │   ├── Infrastructure/
    │   │   ├── Database.cs
    │   │   └── DatabaseFactory.cs
    │   └── Tests/
    │       ├── Api/
    │       │   ├── ApiBehaviourTests/
    │       │   │   ├── CommonApiBehaviourTests.cs
    │       │   │   ├── DevelopmentApiBehaviourTests.cs
    │       │   │   ├── ProductionApiBehaviourTests.cs
    │       │   │   └── BadRequestTests.cs
    │       │   └── Endpoints/Products/
    │       │       ├── ProductsTest.cs
    │       │       ├── CreateProductEndpointTests.cs
    │       │       ├── GetProductEndpointTests.cs
    │       │       ├── GetAllProductsEndpointTests.cs
    │       │       ├── UpdateProductEndpointTests.cs
    │       │       └── DeleteProductsEndpointTests.cs
    │       └── ApiClient/
    │           └── ApiClientTests.cs
    └── FunctionalTests/            # E2E tests against live API (requires docker-compose)
        ├── FunctionalTests.csproj
        ├── BeforeAfterTestConfiguration.cs
        ├── Startup.cs
        ├── Test.cs
        └── Settings/
            ├── ITestSettings.cs
            ├── TestSettings.cs
            └── testsettings.json
```

## Configuration

App settings bind to `TemplateSettings` via `builder.Configuration`. Validated at startup via `TemplateSettingsValidator` using `IValidateOptions`. Application fails to start if required settings are missing.

`Program.cs` is a minimal entrypoint that delegates to `Startup.Run()` for DI setup, Serilog configuration, and endpoint registration.

## Endpoints

Endpoints are organized under `src/Api/Endpoints/` by resource. Each resource has its own subfolder (e.g., `Products/`) containing individual endpoint files. Each endpoint is a `static class` with a `Map(IEndpointRouteBuilder)` method.

Responses use `application/problem+json`. Invalid requests return 400, other errors return 500 with details hidden in production.

## Build & Run

```bash
dotnet run --project src/Api
# or full stack via docker compose (sqlserver → migrator → api):
docker compose up
```

SQL migrations run via the `Persistence.Migrations` project (dbup) and are executed by the `migrator` service in `docker-compose.yml`. The `Dockerfile` and `Dockerfile.migrations` both use `mcr.microsoft.com/dotnet/aspnet:10.0` (non-alpine) for runtime, as `Microsoft.Data.SqlClient` requires full globalization (ICU).

## Tests

```bash
dotnet test
```

Three test projects: `UnitTests`, `IntegrationTests`, `FunctionalTests`, plus `Core.Testing` for shared utilities.

- **UnitTests** — isolated unit tests (AutoFixture, validators)
- **IntegrationTests** — `WebApplicationFactory` + `Testcontainers.MsSql` for EF Core integration (requires Docker socket)
- **FunctionalTests** — E2E tests against a live API; requires `docker compose up` and `ApiUrl` in `Settings/testsettings.json`

CI runs via `ci-docker.sh` which builds `Dockerfile.ci` and mounts the Docker socket to enable Testcontainers.

## Logging Overrides

Serilog level overrides in `appsettings.json` under `MinimumLevel.Override`. Known overrides to silence noisy defaults:

- `"Microsoft.EntityFrameworkCore.Database.Command": "Warning"` — suppresses verbose SQL query logging. Set to `Information` in `appsettings.Development.json` for dev-only SQL visibility.
- `"Microsoft.AspNetCore.Mvc.Infrastructure": "Warning"` — suppresses the useless "No action descriptors found" startup message.

## Coding Conventions

- **No `Async` suffix** — don't name methods `RunAsync`, do `Run`. The `async` modifier on the method body is sufficient.
- **Models over tuples** — use a proper response class instead of `Task<(int, string, string)>`
- **No leading underscore** — name fields `inner`, `client`, `testKdbxPath`, not `_inner`, `_client`, `_testKdbxPath`

## MCP Tools - ALWAYS PREFER

When `mcp__vs-mcp__*` tools are available, ALWAYS use them instead of Grep/Glob/LS:

| Instead of | Use |
|------------|-----|
| `Grep` for symbols | `FindSymbols`, `FindSymbolUsages` |
| `LS` to explore projects | `GetSolutionTree` |
| Reading files to find code | `FindSymbolDefinition` then `Read` |
| Searching for method calls | `GetMethodCallers`, `GetMethodCalls` |

**Why?** MCP tools use Roslyn semantic analysis - 10x faster, 90% fewer tokens.