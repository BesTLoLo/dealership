# Use the .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["DealershipManagement/DealershipManagement.csproj", "DealershipManagement/"]
RUN dotnet restore "DealershipManagement/DealershipManagement.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/DealershipManagement"
RUN dotnet build "DealershipManagement.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "DealershipManagement.csproj" -c Release -o /app/publish

# Use the ASP.NET Core runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create wwwroot/invoices directory for file uploads
RUN mkdir -p /app/wwwroot/invoices

# Create and configure DataProtection-Keys directory
RUN mkdir -p /app/DataProtection-Keys && \
    chown -R www-data:www-data /app/DataProtection-Keys && \
    chmod 700 /app/DataProtection-Keys

# Set proper permissions for the invoices directory
RUN chmod 755 /app/wwwroot/invoices

# Expose the port the app runs on
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "DealershipManagement.dll"]
