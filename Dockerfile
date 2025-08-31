# Multi-stage build for Blazor Server application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Set environment variables for production
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["DealershipManagement.csproj", "./"]
RUN dotnet restore "DealershipManagement.csproj"

# Copy source code
COPY . .
WORKDIR "/src"

# Build the application
RUN dotnet build "DealershipManagement.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "DealershipManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app

# Create wwwroot/invoices directory for file uploads
RUN mkdir -p /app/wwwroot/invoices

# Copy published application
COPY --from=publish /app/publish .

# Set proper permissions for the invoices directory
RUN chmod 755 /app/wwwroot/invoices

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "DealershipManagement.dll"]
