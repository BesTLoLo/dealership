# Deployment Guide for Render.com

This guide will walk you through deploying the Dealership Management application to Render.com using Docker.

## Prerequisites

- A Render.com account
- MongoDB database (MongoDB Atlas recommended for production)
- Git repository with your code

## Step 1: Prepare Your MongoDB Database

### Option A: MongoDB Atlas (Recommended for Production)
1. Go to [MongoDB Atlas](https://www.mongodb.com/atlas)
2. Create a free cluster
3. Create a database user with read/write permissions
4. Get your connection string
5. Whitelist your IP address (or use 0.0.0.0/0 for Render.com)

### Option B: Local MongoDB (Development Only)
- Use the provided `docker-compose.yml` for local development

## Step 2: Deploy to Render.com

### Method 1: Using render.yaml (Recommended)

1. **Push your code to Git repository**
   ```bash
   git add .
   git commit -m "Add Docker and Render.com configuration"
   git push origin main
   ```

2. **Connect to Render.com**
   - Go to [Render.com](https://render.com)
   - Sign in with your Git provider
   - Click "New +" and select "Web Service"

3. **Configure the service**
   - **Name**: `dealership-management`
   - **Environment**: `Docker`
   - **Region**: Choose closest to your users
   - **Branch**: `main`
   - **Root Directory**: Leave empty (root of repository)

4. **Set Environment Variables**
   - `MONGODB_CONNECTION_STRING`: Your MongoDB connection string
   - `ASPNETCORE_ENVIRONMENT`: `Production`
   - `ASPNETCORE_URLS`: `http://+:8080`

5. **Deploy**
   - Click "Create Web Service"
   - Render will automatically build and deploy your application

### Method 2: Manual Configuration

1. **Create Web Service**
   - Environment: `Docker`
   - Build Command: `docker build -t dealership-management .`
   - Start Command: `docker run -p $PORT:8080 dealership-management`

2. **Set Environment Variables**
   ```
   MONGODB_CONNECTION_STRING=mongodb+srv://username:password@cluster.mongodb.net/dealership
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   MONGODB_DATABASE_NAME=DealershipDB
   MONGODB_CARS_COLLECTION=Cars
   MONGODB_INVOICES_COLLECTION=Invoices
   ```

## Step 3: Configure MongoDB Connection

### For MongoDB Atlas:
```
mongodb+srv://username:password@cluster.mongodb.net/dealership?retryWrites=true&w=majority
```

### For Local MongoDB:
```
mongodb://localhost:27017/DealershipDB
```

## Step 4: Verify Deployment

1. **Check Health Endpoint**
   - Visit: `https://your-app-name.onrender.com/health`
   - Should show "Healthy" status

2. **Test Application**
   - Navigate to the main page
   - Try adding a car
   - Verify database operations work

## Environment Variables Reference

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `MONGODB_CONNECTION_STRING` | MongoDB connection string | Yes | - |
| `MONGODB_DATABASE_NAME` | Database name | No | DealershipDB |
| `MONGODB_CARS_COLLECTION` | Cars collection name | No | Cars |
| `MONGODB_INVOICES_COLLECTION` | Invoices collection name | No | Invoices |
| `ASPNETCORE_ENVIRONMENT` | Environment name | No | Production |
| `ASPNETCORE_URLS` | Application URLs | No | http://+:8080 |

## Troubleshooting

### Common Issues

1. **Build Failures**
   - Check Dockerfile syntax
   - Verify all files are committed to Git
   - Check Render.com build logs

2. **Database Connection Errors**
   - Verify MongoDB connection string
   - Check IP whitelist in MongoDB Atlas
   - Ensure database user has correct permissions

3. **Application Won't Start**
   - Check environment variables
   - Verify port configuration
   - Check application logs in Render.com

4. **File Upload Issues**
   - Ensure `wwwroot/invoices` directory exists
   - Check file permissions
   - Verify file size limits

### Debugging

1. **Check Build Logs**
   - Go to your service in Render.com
   - Click on "Events" tab
   - Review build and deployment logs

2. **Check Runtime Logs**
   - Click on "Logs" tab
   - Monitor application logs for errors

3. **Health Check**
   - Visit `/health` endpoint
   - Check for database connectivity issues

## Performance Optimization

1. **Database Indexing**
   - Application automatically creates indexes on startup
   - Monitor query performance in MongoDB Atlas

2. **File Storage**
   - Consider using cloud storage (AWS S3, Azure Blob) for production
   - Update `FileService` to use cloud storage providers

3. **Caching**
   - Implement Redis caching for frequently accessed data
   - Add response caching headers

## Security Considerations

1. **Environment Variables**
   - Never commit `.env` files to Git
   - Use Render.com's environment variable system
   - Rotate database passwords regularly

2. **Database Security**
   - Use strong passwords
   - Enable MongoDB Atlas security features
   - Restrict IP access when possible

3. **Application Security**
   - Keep .NET version updated
   - Monitor security advisories
   - Implement proper input validation

## Scaling

1. **Auto-scaling**
   - Render.com automatically scales based on traffic
   - Monitor resource usage in dashboard

2. **Database Scaling**
   - Upgrade MongoDB Atlas cluster as needed
   - Consider read replicas for high-traffic scenarios

## Monitoring

1. **Render.com Dashboard**
   - Monitor uptime and performance
   - Set up alerts for failures

2. **Application Monitoring**
   - Implement logging and metrics
   - Monitor database performance
   - Track user activity and errors

## Support

- **Render.com Support**: [docs.render.com](https://docs.render.com)
- **MongoDB Atlas Support**: [docs.atlas.mongodb.com](https://docs.atlas.mongodb.com)
- **Application Issues**: Check the main README.md troubleshooting section

---

**Note**: This deployment guide assumes you're using the Docker configuration provided in this repository. For custom deployments, adjust the configuration accordingly.
