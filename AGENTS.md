# Template

.NET 10 layered ASP.NET Core API template with a clean architecture: Api, ApiClient, Application, Core, CrossCutting, Domain, and Persistence projects.

## Architecture

- **Api** — ASP.NET Core minimal API with endpoints organized by resource
- **Application** — Business logic, services, exceptions, and request/response models
- **Domain** — Domain entities (EF Core models)
- **Persistence** — EF Core DbContext, interceptors, and data access
- **Core** — Shared domain interfaces (IAudited, IIdentifiable, IGloballyIdentifiable) and helpers
- **CrossCutting** — Shared concerns: logging extensions, settings, and DI configurators
- **ApiClient** — HTTP client library (stub)

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
├── Dockerfile                      # multi-stage: SDK build → runtime
├── Dockerfile.ci                   # CI runtime image with test dependencies
├── README.md
├── Template.slnx
├── .github/workflows/              # GH Actions
├── src/
│   ├── Api/                        # ASP.NET Core minimal API
│   │   ├── Api.csproj
│   │   ├── Program.cs              # entrypoint, DI, endpoint registration
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Endpoints/
│   │   │   └── ProductEndpoints/
│   │   │       ├── CreateProductEndpoint.cs
│   │   │       ├── DeleteProductEndpoint.cs
│   │   │       ├── GetAllProductsEndpoint.cs
│   │   │       ├── GetProductEndpoint.cs
│   │   │       └── UpdateProductEndpoint.cs
│   │   ├── Extensions/
│   │   │   └── ExceptionHandlerExtensions.cs  # problem+json error handler
│   │   └── Properties/
│   │       └── launchSettings.json
│   ├── ApiClient/                  # HTTP client library (stub)
│   │   └── ApiClient.csproj
│   ├── Application/                # business logic
│   │   ├── Application.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── Exceptions/
│   │   │   ├── TemplateException.cs     # base exception
│   │   │   ├── NotFoundException.cs
│   │   │   └── NotFoundException(T).cs
│   │   ├── Models/
│   │   │   └── Requests/
│   │   │       ├── CreateProductRequest.cs
│   │   │       └── UpdateProductRequest.cs
│   │   └── Services/
│   │       ├── IProductService.cs
│   │       └── ProductService.cs
│   ├── Core/                       # shared domain interfaces and helpers
│   │   ├── Core.csproj
│   │   ├── Domain/
│   │   │   ├── IAudited.cs                # CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
│   │   │   ├── IIdentifiable.cs           # long Id
│   │   │   └── IGloballyIdentifiable.cs   # Guid Guid
│   │   └── Extensions/
│   │       └── TypeExtensions.cs          # helper for type names
│   ├── CrossCutting/               # shared concerns
│   │   ├── CrossCutting.csproj
│   │   ├── DependencyConfigurator.cs
│   │   ├── Logging/
│   │   │   └── ILoggerExtensions.cs       # source-generated log messages
│   │   └── Settings/
│   │       ├── ITemplateSettings.cs
│   │       ├── TemplateSettings.cs        # POCO bound from config
│   │       └── TemplateSettingsValidator.cs  # IValidateOptions for settings
│   ├── Domain/                     # domain entities
│   │   ├── Domain.csproj
│   │   └── Product.cs
│   └── Persistance/                # EF Core data access
│       ├── Persistance.csproj
│       ├── DependencyConfigurator.cs
│       ├── TemplateDbContext.cs
│       └── Interceptors/
│           └── SetAuditInfoSaveChangesInterceptor.cs
└── tests/
    ├── FunctionalTests/
    ├── IntegrationTests/
    └── UnitTests/
```

## Configuration

App settings bind to `TemplateSettings` via `builder.Configuration`. Validated at startup via `TemplateSettingsValidator` using `IValidateOptions`. Application fails to start if required settings are missing.

## Endpoints

Endpoints are organized under `src/Api/Endpoints/` by resource. Each resource has its own subfolder (e.g., `ProductEndpoints/`) containing individual endpoint files (Create, Get, Update, Delete).

Responses use `application/problem+json`. Invalid requests return 400, other errors return 500 with details hidden in production.

## Build & Run

```bash
dotnet run --project src/Api
# or
docker build -t template . && docker run --rm -it template
```

## Tests

```bash
dotnet test
```

Three test projects: `UnitTests`, `IntegrationTests`, `FunctionalTests`.

## Coding Conventions

- **No `Async` suffix** — don't name methods `RunAsync`, do `Run`. The `async` modifier on the method body is sufficient.
- **Models over tuples** — use a proper response class instead of `Task<(int, string, string)>`
- **No leading underscore** — name fields `inner`, `client`, `testKdbxPath`, not `_inner`, `_client`, `_testKdbxPath`