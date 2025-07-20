FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/People.Api/People.Api.csproj", "src/People.Api/"]
COPY ["src/People.Data/People.Data.csproj", "src/People.Data/"]
RUN dotnet restore "./src/People.Api/People.Api.csproj"
COPY . .
WORKDIR "/src/src/People.Api"
RUN dotnet build "./People.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Temporarily remove /p:PublishTrimmed=true for testing
RUN dotnet publish "./People.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish -r linux-x64 --self-contained true /p:PublishSingleFile=true
RUN chmod +x /app/publish/People.Api

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# REMOVE THIS LINE: RUN chmod +x People.Api # No longer needed here
ENTRYPOINT ["./People.Api"]
