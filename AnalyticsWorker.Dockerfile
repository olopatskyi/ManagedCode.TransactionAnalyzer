ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:8.0.100-1-bookworm-slim
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:8.0.0-bookworm-slim
# ARG as constants
ARG PROJECT=ManagedCode.Transactions.AnalyticsWorker
ARG INFRASTRUCTURE=ManagedCode.Transactions.Infrastructure
ARG DATA=ManagedCode.Transactions.Data

FROM ${SDK_IMAGE} AS build-env
ARG PROJECT
ARG INFRASTRUCTURE
ARG DATA
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./NuGet.Config ./
COPY ./${INFRASTRUCTURE}/*.csproj ./${INFRASTRUCTURE}/
COPY ./${DATA}/*.csproj ./${DATA}/
COPY ./${PROJECT}/*.csproj ./${PROJECT}/
WORKDIR /app/${PROJECT}
RUN dotnet restore
WORKDIR /app

# Copy everything else and build
COPY ./${INFRASTRUCTURE}/ ./${INFRASTRUCTURE}/
COPY ./${DATA}/ ./${DATA}/
COPY ./${PROJECT}/ ./${PROJECT}/
WORKDIR /app/${PROJECT}
RUN dotnet publish -c release -o out

# Build runtime image
FROM ${RUNTIME_IMAGE}
ARG PROJECT
WORKDIR /app
COPY --from=build-env /app/${PROJECT}/out ./

EXPOSE 80

# Setup environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
ENV PROJECT=$PROJECT
ENTRYPOINT dotnet $PROJECT.dll
