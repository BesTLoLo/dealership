# PowerShell script to build and test the Docker container locally

Write-Host "🚀 Building Dealership Management Docker container..." -ForegroundColor Green

# Build the Docker image
docker build -t dealership-management .

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Docker build successful!" -ForegroundColor Green
    
    Write-Host "🧪 Testing container locally..." -ForegroundColor Yellow
    
    # Run the container locally
    docker run -d --name dealership-test -p 8080:8080 -p 8081:8081 `
        -e MONGODB_CONNECTION_STRING="mongodb://localhost:27017" `
        -e MONGODB_DATABASE_NAME="DealershipDB" `
        -e ASPNETCORE_ENVIRONMENT="Development" `
        dealership-management
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Container started successfully!" -ForegroundColor Green
        Write-Host "🌐 Application available at:" -ForegroundColor Cyan
        Write-Host "   HTTP:  http://localhost:8080" -ForegroundColor White
        Write-Host "   HTTPS: https://localhost:8081" -ForegroundColor White
        Write-Host "🏥 Health check: http://localhost:8080/health" -ForegroundColor White
        
        Write-Host "`n📋 Container logs:" -ForegroundColor Yellow
        Start-Sleep -Seconds 3
        docker logs dealership-test
        
        Write-Host "`n⏹️  To stop the container, run: docker stop dealership-test" -ForegroundColor Yellow
        Write-Host "🗑️  To remove the container, run: docker rm dealership-test" -ForegroundColor Yellow
    } else {
        Write-Host "❌ Failed to start container" -ForegroundColor Red
    }
} else {
    Write-Host "❌ Docker build failed!" -ForegroundColor Red
    exit 1
}
