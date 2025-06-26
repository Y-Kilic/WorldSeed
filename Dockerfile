FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Src/WorldSeed.sln ./
COPY Src/Common/WorldSeed.Common/WorldSeed.Common.csproj Src/Common/WorldSeed.Common/
COPY Src/Core/WorldSeed.Domain/WorldSeed.Domain.csproj Src/Core/WorldSeed.Domain/
COPY Src/Core/WorldSeed.Application/WorldSeed.Application.csproj Src/Core/WorldSeed.Application/
COPY Src/Infrastructure/WorldSeed.Infrastructure/WorldSeed.Infrastructure.csproj Src/Infrastructure/WorldSeed.Infrastructure/
COPY Src/Infrastructure/WorldSeed.Persistence/WorldSeed.Persistence.csproj Src/Infrastructure/WorldSeed.Persistence/
COPY Src/Presentation/WorldSeed.Api/WorldSeed.Api.csproj Src/Presentation/WorldSeed.Api/
RUN dotnet restore WorldSeed.sln

# copy everything else and publish
COPY . .
RUN dotnet publish Src/Presentation/WorldSeed.Api/WorldSeed.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Application listens on port 8080 by default
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WorldSeed.Api.dll"]
