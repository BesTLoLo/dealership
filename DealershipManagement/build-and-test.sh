#!/bin/bash

# Bash script to build and test the Docker container locally

echo "🚀 Building Dealership Management Docker container..."

# Build the Docker image
docker build -t dealership-management .

if [ $? -eq 0 ]; then
    echo "✅ Docker build successful!"
    
    echo "🧪 Testing container locally..."
    
    # Run the container locally
    docker run -d --name dealership-test -p 8080:8080 -p 8081:8081 \
        -e MONGODB_CONNECTION_STRING="mongodb://localhost:27017" \
        -e MONGODB_DATABASE_NAME="DealershipDB" \
        -e ASPNETCORE_ENVIRONMENT="Development" \
        dealership-management
    
    if [ $? -eq 0 ]; then
        echo "✅ Container started successfully!"
        echo "🌐 Application available at:"
        echo "   HTTP:  http://localhost:8080"
        echo "   HTTPS: https://localhost:8081"
        echo "🏥 Health check: http://localhost:8080/health"
        
        echo ""
        echo "📋 Container logs:"
        sleep 3
        docker logs dealership-test
        
        echo ""
        echo "⏹️  To stop the container, run: docker stop dealership-test"
        echo "🗑️  To remove the container, run: docker rm dealership-test"
    else
        echo "❌ Failed to start container"
    fi
else
    echo "❌ Docker build failed!"
    exit 1
fi
