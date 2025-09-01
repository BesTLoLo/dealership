using DealershipManagement.Components;
using DealershipManagement.Data;
using DealershipManagement.Repositories;
using DealershipManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Blazor.Bootstrap services
builder.Services.AddBlazorBootstrap();

// Configure MongoDB with environment variables from Render
var mongoDbSettings = new MongoDbSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? 
                      builder.Configuration.GetConnectionString("MongoDb") ?? 
                      "mongodb://localhost:27017",
    DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? 
                   builder.Configuration["MongoDb:DatabaseName"] ?? 
                   "DealershipDB",
    CarsCollectionName = Environment.GetEnvironmentVariable("MONGODB_CARS_COLLECTION") ?? 
                         builder.Configuration["MongoDb:CarsCollectionName"] ?? 
                         "Cars",
    InvoicesCollectionName = Environment.GetEnvironmentVariable("MONGODB_INVOICES_COLLECTION") ?? 
                             builder.Configuration["MongoDb:InvoicesCollectionName"] ?? 
                             "Invoices"
};

builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = mongoDbSettings.ConnectionString;
    options.DatabaseName = mongoDbSettings.DatabaseName;
    options.CarsCollectionName = mongoDbSettings.CarsCollectionName;
    options.InvoicesCollectionName = mongoDbSettings.InvoicesCollectionName;
});

// Register services
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IDataSeederService, DataSeederService>();
builder.Services.AddScoped<DatabaseInitializer>();

var app = builder.Build();

// Initialize database and seed sample data
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeederService>();
    await seeder.SeedSampleDataAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
