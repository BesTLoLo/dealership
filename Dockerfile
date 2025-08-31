# Multi-stage build for Blazor Server application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Set environment variables for production
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["DealershipManagement/DealershipManagement.csproj", "./"]
RUN dotnet restore "DealershipManagement.csproj"

# Copy source code
COPY . .
WORKDIR "/src/DealershipManagement"

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

# Create and configure DataProtection-Keys directory
RUN mkdir -p /app/DataProtection-Keys && \
    chown -R www-data:www-data /app/DataProtection-Keys && \
    chmod 700 /app/DataProtection-Keys

# Set proper permissions for the invoices directory
RUN chmod 755 /app/wwwroot/invoices

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "DealershipManagement.dll"]
