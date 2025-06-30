# WorldSeed
To help develop new .NET applications.

# Main goal:
1# To create a secure, fast, loosely coupled template that can be used as base for any web project.

# Other goals: 
1# To have only one code base to maintain and everything else should be an extension. This way any improvements you make to the framework can be shared between projects, saving time and money.

# The template:
As a developer I dislike reinventing the wheel every time I am assigned to work on a project, I wish to create a template that can be used to quickly create new projects, without having to sacrifice security or the quality of the code.

# Features:
- Clean architecture
- Repository pattern
- Unit Of work pattern
- Service pattern
- JWT bearer token authentication
- REST API
- Cross platform
- API base URL configurable via `ApiSettings:Url` in `appsettings.json`

# Web client for the API
https://github.com/Y-Kilic/Worldseed.Client.Blazor

Credits to:
https://github.com/jasontaylordev/NorthwindTraders (For his example and inspiration)

## Running tests

This repository targets **.NET 8**. Make sure the appropriate SDK is installed
or use the version specified in `global.json`.

To execute the unit tests run:

```bash
dotnet test Src/WorldSeed.sln -c Release
```

## Deployment on Fly.io

A `Dockerfile` is provided to containerize the API. Build and run locally with:

```bash
docker build -t worldseed .
docker run -p 8080:8080 worldseed
```

The service listens on port `8080`. When deploying to Fly.io, ensure `fly.toml` sets the `internal_port` to `8080` so incoming traffic is forwarded correctly.

Swagger UI is enabled in all environments. Visiting the root URL or `/swagger` shows the interactive API documentation.
