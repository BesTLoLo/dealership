# Dealership Management System

A comprehensive Blazor Server application for managing car dealership operations, built with .NET 9 and MongoDB. This system provides complete inventory management, maintenance tracking, and sales monitoring capabilities.

## 🚗 Features

- **Complete Car Lifecycle Management**: Track cars from purchase through maintenance to sale
- **User Authentication**: Secure login system with username/password authentication
- **Automatic Financial Calculations**: Built-in tax calculations (10% rate) and final price computations
- **Comprehensive Maintenance Tracking**: Detailed service logs with invoice file attachments
- **Advanced Search & Filtering**: Search by VIN, stock number, make, model, year with advanced filters
- **File Management System**: Upload, store, and retrieve invoice documents (PDF, images, Word docs)
- **Responsive Modern UI**: Bootstrap 5-based interface with Blazor components
- **Real-time Data**: Interactive Blazor Server with immediate UI updates
- **Database Indexing**: Optimized MongoDB performance with strategic indexes

## 🛠️ Technology Stack

- **Frontend**: Blazor Server with .NET 9
- **Database**: MongoDB with MongoDB.Driver 3.4.3
- **UI Framework**: Blazor.Bootstrap 3.4.0
- **File Storage**: Local file system with configurable paths
- **Architecture**: Repository pattern with service layer
- **Validation**: Data Annotations with client-side validation

## 📋 Prerequisites

- .NET 9 SDK
- MongoDB (local installation or cloud instance)
- Visual Studio 2022 or VS Code with C# extensions
- Modern web browser

## 🚀 Installation & Setup

### Option 1: Docker (Recommended)

#### Using Docker Compose (Local Development)
```bash
# Clone the repository
git clone <repository-url>
cd DealershipManagement

# Start the application with MongoDB
docker-compose up -d

# The application will be available at:
# - HTTP:  http://localhost:8080
# - HTTPS: https://localhost:8081
```

#### Manual Docker Build
```bash
# Build the Docker image
docker build -t dealership-management .

# Run the container
docker run -d --name dealership-app -p 8080:8080 -p 8081:8081 \
  -e MONGODB_CONNECTION_STRING="mongodb://localhost:27017" \
  dealership-management
```

#### Using Build Scripts
```bash
# Windows PowerShell
.\build-and-test.ps1

# Linux/macOS
chmod +x build-and-test.sh
./build-and-test.sh
```

### Option 2: Traditional Installation

#### 1. Clone the Repository
```bash
git clone <repository-url>
cd DealershipManagement
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Configure MongoDB

#### Option 1: Environment Variables (Recommended)
Create a `.env` file in the project root with your MongoDB configuration:

```bash
# Copy env.example to .env and update values
cp env.example .env
```

Then edit the `.env` file:
```bash
# MongoDB Connection String
MONGODB_CONNECTION_STRING=mongodb://localhost:27017

# Database Name
MONGODB_DATABASE_NAME=DealershipDB

# Collection Names (optional - these have defaults)
MONGODB_CARS_COLLECTION=Cars
MONGODB_INVOICES_COLLECTION=Invoices
```

#### Option 2: Direct Configuration
Alternatively, update the `appsettings.json` file with your MongoDB connection details:

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "DealershipDB",
    "CarsCollectionName": "Cars",
    "InvoicesCollectionName": "Invoices"
  }
}
```

**Note**: Environment variables take precedence over appsettings.json values.

### 4. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

**Note**: The system automatically initializes the database and seeds sample data on first run.

## 🗄️ Database Schema

### Car Collection
```json
{
  "_id": "ObjectId",
  "VIN": "string (unique)",
  "Make": "string",
  "Model": "string", 
  "Year": "integer",
  "StockNumber": "string (unique)",
  "BuyPrice": "decimal",
  "BuyDate": "DateTime",
  "SellPrice": "decimal",
  "SellDate": "DateTime?",
  "Tax": "decimal (calculated)",
  "FinalSellPrice": "decimal (calculated)",
  "FinalBuyPrice": "decimal (calculated)",
  "MaintenanceLogs": "array of MaintenanceLog objects"
}
```

### Maintenance Log (Embedded Document)
```json
{
  "Id": "string (GUID)",
  "PartNumber": "string",
  "PartDescription": "string",
  "Price": "decimal",
  "FinalPrice": "decimal (calculated)",
  "Date": "DateTime",
  "InvoiceFilePath": "string (file path)"
}
```

## 🎯 Usage Guide

### Adding a New Car
1. Navigate to **Cars** page
2. Click **"Add New Car"** button
3. Fill in required fields:
   - VIN (unique identifier)
   - Stock Number (unique)
   - Make, Model, Year
   - Buy Price
4. System automatically calculates tax and final buy price
5. Click **"Add Car"** to save

### Managing Maintenance Logs
1. View car details from the Cars list
2. Click **"Add Log"** in the Maintenance Logs section
3. Enter part information:
   - Part Number
   - Part Description
   - Price
   - Date
4. Upload invoice file (optional)
5. Save the maintenance log

### Advanced Car Search
1. Go to **Search** page
2. Use the search bar for quick searches
3. Click **"Advanced Filters"** for detailed filtering:
   - Status (Available/Sold)
   - Make/Model/Year
   - Date ranges
4. View paginated results
5. Click on cars to view full details

### Selling a Car
1. Navigate to car details
2. Click **"Sell"** button
3. Enter sell price and date
4. System automatically calculates final sell price with tax
5. Car status updates to "Sold"

## ⚙️ Configuration

### Environment Variables
The application supports configuration through environment variables for enhanced security and flexibility:

- **MONGODB_CONNECTION_STRING**: MongoDB connection string (default: mongodb://localhost:27017)
- **MONGODB_DATABASE_NAME**: Database name (default: DealershipDB)
- **MONGODB_CARS_COLLECTION**: Cars collection name (default: Cars)
- **MONGODB_INVOICES_COLLECTION**: Invoices collection name (default: Invoices)
- **AUTH_USERNAME**: Username for login authentication
- **AUTH_PASSWORD**: Password for login authentication

#### Using .env File
1. Copy `env.example` to `.env`
2. Update the values in `.env`
3. The application automatically loads these environment variables

#### Using System Environment Variables
You can also set these variables at the system level:
```bash
# Windows (PowerShell)
$env:MONGODB_CONNECTION_STRING="mongodb://your-server:27017"

# Linux/macOS
export MONGODB_CONNECTION_STRING="mongodb://your-server:27017"
```

### File Upload Settings
- **Maximum file size**: 10MB
- **Supported formats**: PDF, JPG, JPEG, PNG, DOC, DOCX
- **Storage location**: `wwwroot/invoices/`
- **File naming**: Unique GUID-based naming to prevent conflicts

### Tax Configuration
- **Default tax rate**: 10% (configurable in `CarService.cs`)
- **Automatic calculation**: Applied to both buy and sell prices
- **Final price formula**: Base price + (Base price × Tax rate)

### Database Indexing
- **Performance indexes**: VIN, StockNumber, Make, Model, Year, BuyDate, SellDate
- **Unique constraints**: VIN and StockNumber fields
- **Automatic creation**: Indexes created on application startup

## 🏗️ Project Structure

```
DealershipManagement/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor          # Main application layout
│   │   ├── MainLayout.razor.css      # Layout-specific styles
│   │   ├── NavMenu.razor             # Navigation menu
│   │   └── NavMenu.razor.css         # Navigation styles
│   └── Pages/
│       ├── Home.razor                # Dashboard/home page
│       ├── Cars.razor                # Car management interface
│       ├── CarDetails.razor          # Individual car view
│       ├── CarEdit.razor             # Car editing form
│       ├── CarSell.razor             # Car sale interface
│       ├── Search.razor              # Advanced search page
│       └── Error.razor               # Error handling page
├── Data/
│   ├── MongoDbSettings.cs            # MongoDB configuration
│   ├── MongoDbContext.cs             # Database context
│   └── DatabaseInitializer.cs        # Database setup and indexing
├── Models/
│   ├── Car.cs                        # Car entity model
│   ├── MaintenanceLog.cs             # Maintenance log model
│   └── SearchFilters.cs              # Search criteria model
├── Repositories/
│   ├── ICarRepository.cs             # Car repository interface
│   └── CarRepository.cs              # MongoDB implementation
├── Services/
│   ├── ICarService.cs                # Car business logic interface
│   ├── CarService.cs                 # Car business logic implementation
│   ├── IMaintenanceService.cs        # Maintenance service interface
│   ├── MaintenanceService.cs         # Maintenance service implementation
│   ├── IFileService.cs               # File handling interface
│   ├── FileService.cs                # File handling implementation
│   ├── IDataSeederService.cs         # Data seeding interface
│   └── DataSeederService.cs          # Sample data seeding
├── wwwroot/
│   ├── invoices/                     # Invoice file storage
│   ├── lib/                          # External libraries (Bootstrap)
│   └── app.css                       # Application styles
└── Program.cs                        # Application entry point
```

## 🔧 Development

### Adding New Features
1. **Models**: Create entity models in `Models/` folder
2. **Repositories**: Add repository interfaces and MongoDB implementations
3. **Services**: Create service layer for business logic
4. **Components**: Build Blazor components for the UI
5. **Registration**: Register new services in `Program.cs`

### Code Patterns
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic encapsulation
- **Dependency Injection**: Service registration and resolution
- **Async/Await**: Non-blocking database operations
- **Data Validation**: Client and server-side validation

### Database Operations
- **CRUD Operations**: Create, Read, Update, Delete for cars
- **Search & Filtering**: Flexible querying with MongoDB
- **File Handling**: Secure file upload and storage
- **Indexing**: Performance optimization for common queries

## 📊 Performance Features

- **MongoDB Indexing**: Strategic indexes on frequently queried fields
- **Pagination**: Efficient handling of large result sets
- **Async Operations**: Non-blocking I/O throughout the application
- **File Optimization**: Unique naming and size validation
- **Memory Management**: Efficient object lifecycle management

## 🔒 Security Features

- **Input Validation**: Comprehensive data validation using Data Annotations
- **File Type Validation**: Strict file type checking for uploads
- **Secure File Storage**: Isolated file storage with path validation
- **Data Sanitization**: Clean input handling and output encoding
- **Access Control**: Proper separation of concerns in service layers

## 🐛 Troubleshooting

### Common Issues

1. **MongoDB Connection Error**
   - Verify MongoDB service is running
   - Check connection string in `appsettings.json`
   - Ensure network access to MongoDB instance
   - Verify database name and collection names

2. **File Upload Failures**
   - Check file size (max 10MB)
   - Verify supported file types
   - Ensure `wwwroot/invoices/` directory exists
   - Check file permissions

3. **Build Errors**
   - Run `dotnet restore` to restore packages
   - Verify .NET 9 SDK installation
   - Check for missing dependencies
   - Clear `obj/` and `bin/` folders if needed

4. **Database Initialization Issues**
   - Check MongoDB connection
   - Verify user permissions
   - Check console output for error messages
   - Ensure unique constraints aren't violated

### Performance Issues
- Verify MongoDB indexes are created
- Check for large result sets without pagination
- Monitor file storage usage
- Review database query patterns

## 📈 Sample Data

The system automatically seeds sample data including:
- **3 sample cars** with different makes and models
- **Maintenance logs** for each vehicle
- **Various statuses** (available and sold)
- **Realistic pricing** and dates

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow C# coding conventions
- Use async/await for database operations
- Implement proper error handling
- Add validation for new models
- Update documentation for new features

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Open an issue in the repository
- Check the troubleshooting section above
- Review the code examples in the Models and Services folders
- Ensure all prerequisites are met

## 🚀 Deployment

### Render.com Deployment
This application is ready for deployment on Render.com using Docker. See [DEPLOYMENT.md](./DEPLOYMENT.md) for detailed instructions.

#### Quick Deploy Steps:
1. Push your code to Git repository
2. Connect to Render.com
3. Create new Web Service with Docker environment
4. Set environment variables (especially `MONGODB_CONNECTION_STRING`)
5. Deploy!

#### Required Environment Variables for Production:
- `MONGODB_CONNECTION_STRING`: Your MongoDB connection string
- `AUTH_USERNAME`: Username for authentication
- `AUTH_PASSWORD`: Password for authentication
- `ASPNETCORE_ENVIRONMENT`: Production
- `ASPNETCORE_URLS`: http://+:8080

### Docker Support
- **Multi-stage Dockerfile** optimized for production
- **Docker Compose** for local development
- **Health check endpoint** at `/health`
- **Non-root user** for security
- **Automatic file directory creation**

## 🔄 Version History

- **v1.1.0**: Docker and Deployment Support
  - Docker containerization
  - Render.com deployment configuration
  - Health check endpoint
  - Environment variable configuration
  - Production-ready Docker setup

- **v1.0.0**: Initial release with core functionality
  - Car management system
  - Maintenance tracking
  - File upload capabilities
  - Advanced search and filtering
  - MongoDB integration
  - Bootstrap 5 UI

---

**Built with ❤️ using .NET 9, Blazor Server, and MongoDB**
