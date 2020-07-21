# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy and publish app and libraries
COPY . .
RUN dotnet build -c release
RUN dotnet publish src/ConsumerWorker/ConsumerWorker.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ConsumerWorker.dll"]