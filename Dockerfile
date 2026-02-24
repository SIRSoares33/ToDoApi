# https://aka.ms/customizecontainer
# This stage is used when running from Visual Studio in fast mode (Debug)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["ToDo.Api/Todo.Api.csproj", "ToDo.Api/"]
COPY ["ToDo.Application/ToDo.Application.csproj", "ToDo.Application/"]
COPY ["ToDo.Domain/ToDo.Domain.csproj", "ToDo.Domain/"]
COPY ["ToDo.Infrastructure/ToDo.Infrastructure.csproj", "ToDo.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ToDo.Api/Todo.Api.csproj"

# Copy remaining source code
COPY . .

# Build
WORKDIR "/src/ToDo.Api"
RUN dotnet build "Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Final stage (Production)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.Api.dll"]