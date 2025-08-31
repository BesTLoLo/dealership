# Dealership Management System

A comprehensive Blazor Server application for managing car dealership operations, built with .NET 9 and MongoDB.

## Features

- **Car Management**: Add, edit, delete, and track cars from purchase to sale
- **Automatic Calculations**: Tax calculations and final price computations
- **Maintenance Tracking**: Detailed service logs with invoice file attachments
- **Advanced Search**: Search by VIN, stock number, make, model, or year with filters
- **File Management**: Upload and store invoice documents
- **Responsive UI**: Modern Bootstrap-based interface

## Technology Stack

- **Frontend**: Blazor Server with .NET 9
- **Database**: MongoDB with MongoDB.Driver
- **UI Framework**: Bootstrap 5
- **File Storage**: Local file system with configurable paths

## Prerequisites

- .NET 9 SDK
- MongoDB (local installation or cloud instance)
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd DealershipManagement
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Configure MongoDB
Update the `appsettings.json` file with your MongoDB connection details:

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

### 4. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Database Schema

### Car Collection
```json
{
  "_id": "ObjectId",
  "VIN": "string",
  "Make": "string",
  "Model": "string",
  "Year": 2021,
  "StockNumber": "string",
  "BuyPrice": 12000.0,
  "BuyDate": "2024-08-01T00:00:00Z",
  "SellPrice": 15000.0,
  "SellDate": "2024-09-10T00:00:00Z",
  "Tax": 1500.0,
  "FinalSellPrice": 16500.0,
  "FinalBuyPrice": 13500.0,
  "MaintenanceLogs": [...]
}
```

### Maintenance Log (Embedded)
```json
{
  "Id": "uuid",
  "PartNumber": "12345",
  "PartDescription": "Brake Pads",
  "Price": 200.0,
  "FinalPrice": 220.0,
  "Date": "2024-08-15T00:00:00Z",
  "InvoiceFilePath": "/invoices/brakepads123.pdf"
}
```

## Usage

### Adding a New Car
1. Navigate to the Cars page
2. Click "Add New Car"
3. Fill in the required fields (VIN, Stock Number, Make, Model, Year)
4. Set buy price and date
5. Click "Add Car"

### Managing Maintenance Logs
1. View car details
2. Click "Add Log" in the Maintenance Logs section
3. Enter part information and upload invoice files
4. Save the maintenance log

### Searching Cars
1. Go to the Search page
2. Enter search terms or use advanced filters
3. View paginated results
4. Click on cars to view details

### Selling a Car
1. Navigate to car details
2. Click "Sell" button
3. Enter sell price and date
4. System automatically calculates final sell price with tax

## Configuration

### File Upload Settings
- Maximum file size: 10MB
- Supported formats: PDF, JPG, JPEG, PNG, DOC, DOCX
- Storage location: `wwwroot/invoices/`

### Tax Rate
- Default tax rate: 10%
- Configurable in `CarService.cs`

## Development

### Project Structure
```
DealershipManagement/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   └── Pages/
│       ├── Home.razor
│       ├── Cars.razor
│       ├── Search.razor
│       └── CarDetails.razor
├── Data/
│   ├── MongoDbSettings.cs
│   └── MongoDbContext.cs
├── Models/
│   ├── Car.cs
│   ├── MaintenanceLog.cs
│   └── SearchFilters.cs
├── Repositories/
│   ├── ICarRepository.cs
│   └── CarRepository.cs
├── Services/
│   ├── ICarService.cs
│   ├── CarService.cs
│   ├── IMaintenanceService.cs
│   ├── MaintenanceService.cs
│   ├── IFileService.cs
│   └── FileService.cs
└── wwwroot/
    └── invoices/
```

### Adding New Features
1. Create models in the `Models/` folder
2. Add repository interfaces and implementations
3. Create service layer for business logic
4. Build Blazor components for the UI
5. Register services in `Program.cs`

## Performance Considerations

- MongoDB indexes on VIN and StockNumber fields
- Pagination for large result sets
- Efficient file storage with unique naming
- Async/await patterns throughout

## Security Features

- Input validation and sanitization
- File type validation
- Secure file storage paths
- Data validation using Data Annotations

## Troubleshooting

### Common Issues

1. **MongoDB Connection Error**
   - Verify MongoDB is running
   - Check connection string in `appsettings.json`
   - Ensure network access to MongoDB instance

2. **File Upload Failures**
   - Check file size limits
   - Verify supported file types
   - Ensure `wwwroot/invoices/` directory exists

3. **Build Errors**
   - Run `dotnet restore`
   - Verify .NET 9 SDK installation
   - Check for missing dependencies

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions, please open an issue in the repository.
