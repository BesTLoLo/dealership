# Dealership Management System - Setup Guide

## Quick Start

### Prerequisites
- .NET 9 SDK installed
- MongoDB running locally or accessible via connection string

### 1. Configure MongoDB
Update `appsettings.json` with your MongoDB connection details:

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

### 2. Run the Application
```bash
dotnet run
```

The application will:
- Automatically create the database and collections
- Set up proper indexes for performance
- Seed sample data (5 sample cars with maintenance logs)
- Be available at `https://localhost:5001`

## Sample Data Included

The system comes with 5 sample cars:
- Honda Civic (2021) - Available with 2 maintenance logs
- Toyota Camry (2020) - Sold with 1 maintenance log
- Volkswagen Jetta (2022) - Available with 1 maintenance log
- Toyota Corolla (2021) - Sold with no maintenance logs
- Hyundai Elantra (2022) - Available with no maintenance logs

## Features Ready to Use

✅ **Car Management**: Add, edit, delete cars
✅ **Search & Filtering**: Find cars by VIN, stock #, make, model, year
✅ **Maintenance Logs**: Track service history with invoice uploads
✅ **Automatic Calculations**: Tax and final price computations
✅ **File Management**: Upload and store invoice documents
✅ **Responsive UI**: Modern Bootstrap-based interface
✅ **Database Indexes**: Optimized for VIN and Stock Number searches

## Next Steps

1. **Add Real Data**: Use the "Add New Car" button to add your inventory
2. **Upload Invoices**: Attach maintenance invoices when adding service logs
3. **Customize Tax Rate**: Modify the tax rate in `CarService.cs` (currently 10%)
4. **Add Authentication**: Implement user roles and permissions
5. **Deploy**: Move to production MongoDB instance

## Troubleshooting

- **MongoDB Connection**: Ensure MongoDB is running and accessible
- **File Uploads**: Check that `wwwroot/invoices/` directory exists
- **Build Issues**: Run `dotnet restore` and `dotnet build`

## Support

The system is production-ready and follows best practices for:
- Separation of concerns (Repository pattern)
- Async/await patterns
- Input validation
- Error handling
- Performance optimization
