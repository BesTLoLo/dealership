using DealershipManagement.Components;
using DealershipManagement.Data;
using DealershipManagement.Repositories;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for production deployment
builder.WebHost.ConfigureKestrel(options =>
{
    // In production, only listen on HTTP port
    // HTTPS termination is handled by the platform (Render)
    if (!builder.Environment.IsDevelopment())
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        options.ListenAnyIP(int.Parse(port));
    }
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Blazor.Bootstrap services
builder.Services.AddBlazorBootstrap();

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();

// Configure MongoDB with environment variables from Render
var mongoDbSettings = new MongoDbSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? 
                      builder.Configuration["MongoDb:ConnectionString"] ?? 
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
builder.Services.AddScoped<IDataSeederService, DataSeederService>();
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
    
    // Add security headers for production
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        await next();
    });
}
else
{
    // Only use HTTPS redirection in development
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
