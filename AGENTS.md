# Template

.NET 10 layered ASP.NET Core API template with a clean architecture: Api, ApiClient, Application, Core, CrossCutting, Domain, and Persistence projects.

## Architecture

- **Api** — ASP.NET Core minimal API with endpoints organized by resource
- **Application** — Business logic, services, exceptions, and request/response models
- **Domain** — Domain entities (EF Core models)
- **Persistence** — EF Core DbContext, interceptors, and data access
- **Persistence.Migrations** — dbup-based SQL migration runner
- **Core** — Shared domain interfaces (IAudited, IIdentifiable, IGloballyIdentifiable), Builder pattern, and helpers
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
│   │   │   ├── Image/
│   │   │   │   └── GetImageEndpoint.cs
│   │   │   ├── ImageGroup/
│   │   │   │   ├── SaveImageGroupEndpoint.cs
│   │   │   │   ├── GetImageGroupEndpoint.cs
│   │   │   │   ├── DeleteImageGroupEndpoint.cs
│   │   │   │   └── DeleteImageGroupV2Endpoint.cs
│   │   │   └── Export/
│   │   │       └── ExportEndpoint.cs
│   │   ├── Exceptions/
│   │   │   └── ApiException.cs
│   │   ├── Extensions/
│   │   │   ├── EndpointExtensions.cs
│   │   │   ├── ExceptionHandlerExtensions.cs
│   │   │   ├── RoutingEndpointConventionBuilderExtensions.cs
│   │   │   └── WebApplicationExtensions.cs
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
│   │   ├── Extensions/
│   │   │   └── SpreadCheetahExtensions.cs
│   │   ├── Exceptions/
│   │   │   ├── AppException.cs            # base exception
│   │   │   ├── NotFoundException.cs
│   │   │   └── NotFoundException(T).cs
│   │   ├── Models/
│   │   │   ├── DataTransferObjects/
│   │   │   │   ├── Entity.cs
│   │   │   │   ├── Image.cs
│   │   │   │   └── ImageGroup.cs
│   │   │   └── Responses/
│   │   │       └── File.cs
│   │   └── Services/
│   │       ├── ExcelExportService.cs
│   │       ├── IMyService.cs
│   │       ├── ImageService.cs
│   │       ├── MyService.cs
│   │       └── TestService.cs
│   ├── Core/                       # shared domain interfaces and helpers
│   │   ├── Core.csproj
│   │   ├── IFactory.cs
│   │   ├── Builders/
│   │   │   ├── Builder.cs
│   │   │   ├── BuilderWithInstance.cs
│   │   │   ├── BuilderWithValues.cs
│   │   │   └── IBuilder.cs
│   │   ├── Domain/
│   │   │   ├── IAudited.cs                # CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
│   │   │   ├── IIdentifiable.cs           # long Id
│   │   │   └── IGloballyIdentifiable.cs   # Guid Guid
│   │   └── Extensions/
│   │       ├── DateTimeExtensions.cs
│   │       ├── IEnumerableExtensions.cs
│   │       ├── StringExtensions.cs
│   │       └── TypeExtensions.cs          # helper for type names
│   ├── CrossCutting/               # shared concerns
│   │   ├── CrossCutting.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── Logging/
│   │   │   └── ILoggerExtensions.cs       # source-generated log messages
│   │   └── Settings/
│   │       ├── IApiSettings.cs
│   │       ├── ApiSettings.cs             # POCO bound from config (Url, SqlServerConnectionString, ImagesStoragePath, ImagesRequestPath)
│   │       └── ApiSettingsValidator.cs    # IValidateOptions for settings
│   ├── Domain/                     # domain entities
│   │   ├── Domain.csproj
│   │   ├── ITestable.cs
│   │   ├── Models/
│   │   │   ├── Entity.cs
│   │   │   ├── Customer.cs
│   │   │   ├── Image.cs
│   │   │   ├── ImageFileExtension.cs
│   │   │   ├── ImageGroup.cs
│   │   │   ├── ImageResolution.cs
│   │   │   ├── ImageType.cs
│   │   │   ├── Order.cs
│   │   │   ├── OrderLine.cs
│   │   │   ├── Product.cs
│   │   │   └── User.cs
│   │   └── Validators/
│   │       └── ProductValidator.cs
│   ├── Persistence/                # EF Core data access
│   │   ├── Persistence.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/
│   │   │   ├── EntityConfiguration.cs
│   │   │   ├── ImageConfiguration.cs
│   │   │   ├── ImageFileExtensionConfiguration.cs
│   │   │   ├── ImageGroupConfiguration.cs
│   │   │   ├── ImageResolutionConfiguration.cs
│   │   │   ├── ImageTypeConfiguration.cs
│   │   │   └── UserConfiguration.cs
│   │   └── Interceptors/
│   │       └── SetAuditInfoSaveChangesInterceptor.cs
│   └── Persistence.Migrations/     # dbup SQL migration runner
│       ├── Persistence.Migrations.csproj
│       ├── Program.cs
│       ├── Migrator.cs
│       ├── Extensions/
│       │   └── DatabaseUpgradeResultExtensions.cs
│       └── Scripts/
│           └── 0001_Initial.sql
└── tests/
    ├── Core.Testing/               # shared test utilities (builders, models, validators)
    │   ├── Core.Testing.csproj
    │   ├── Builders/
    │   │   ├── ProductBuilder.cs
    │   │   └── ProblemDetailsBuilder.cs
    │   ├── Extensions/
    │   │   ├── AutoFixture/StringGuardClauseAssertion.cs
    │   │   ├── HttpResponseMessageExtensions.cs
    │   │   └── ProblemDetailsExtensions.cs
    │   ├── Helpers/
    │   │   └── TestFileHelper.cs
    │   ├── Models/
    │   │   ├── Entity.cs
    │   │   ├── Image.cs
    │   │   └── ImageGroup.cs
    │   └── Validators/
    │       ├── ProblemDetailsValidator.cs
    │       └── TraceIdValidator.cs
    ├── FunctionalTests/            # E2E tests against live API (requires docker-compose)
    ├── IntegrationTests/           # WebApplicationFactory + Testcontainers.MsSql
    └── UnitTests/                  # isolated unit tests
```

## Configuration

App settings bind to `ApiSettings` via `builder.Configuration`. Validated at startup via `ApiSettingsValidator` using `IValidateOptions`. Application fails to start if required settings are missing (`Url`, `SqlServerConnectionString`, `ImagesStoragePath`, `ImagesRequestPath`).

`Program.cs` is a minimal entrypoint that delegates to `Startup.Run()` for DI setup, Serilog configuration, and endpoint registration.

## Endpoints

Endpoints are organized under `src/Api/Endpoints/` by resource. Each resource has its own subfolder (e.g., `ImageGroup/`) containing individual endpoint files. Each endpoint is a `static class` with a `Map(IEndpointRouteBuilder)` method.

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

## Coding Conventions

- **No `Async` suffix** — don't name methods `RunAsync`, do `Run`. The `async` modifier on the method body is sufficient.
- **Models over tuples** — use a proper response class instead of `Task<(int, string, string)>`
- **No leading underscore** — name fields `inner`, `client`, `testKdbxPath`, not `_inner`, `_client`, `_testKdbxPath`