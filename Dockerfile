FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Src/WorldSeed.sln ./
COPY Src/Common/WorldSeed.Common/WorldSeed.Common.csproj Common/WorldSeed.Common/
COPY Src/Core/WorldSeed.Domain/WorldSeed.Domain.csproj Core/WorldSeed.Domain/
COPY Src/Core/WorldSeed.Application/WorldSeed.Application.csproj Core/WorldSeed.Application/
COPY Src/Infrastructure/WorldSeed.Infrastructure/WorldSeed.Infrastructure.csproj Infrastructure/WorldSeed.Infrastructure/
COPY Src/Infrastructure/WorldSeed.Persistence/WorldSeed.Persistence.csproj Infrastructure/WorldSeed.Persistence/
COPY Src/Presentation/WorldSeed.Api/WorldSeed.Api.csproj Presentation/WorldSeed.Api/
COPY Tests/WorldSeed.Tests/WorldSeed.Tests.csproj ../Tests/WorldSeed.Tests/
RUN dotnet restore WorldSeed.sln

# copy everything else and publish
COPY . .
RUN dotnet publish Src/Presentation/WorldSeed.Api/WorldSeed.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Application listens on the port specified by $PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 8080

ENTRYPOINT ["dotnet", "WorldSeed.Api.dll"]
