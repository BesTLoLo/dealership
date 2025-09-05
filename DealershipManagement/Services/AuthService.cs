using Microsoft.AspNetCore.Authentication;

namespace DealershipManagement.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public AuthService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        // Get credentials from environment variables
        var envUsername = Environment.GetEnvironmentVariable("AUTH_USERNAME") ?? 
                         _configuration["Authentication:Username"];
        var envPassword = Environment.GetEnvironmentVariable("AUTH_PASSWORD") ?? 
                         _configuration["Authentication:Password"];

        if (string.IsNullOrEmpty(envUsername) || string.IsNullOrEmpty(envPassword))
        {
            return false;
        }

        // Simple validation - in production, use proper password hashing
        return await Task.FromResult(username == envUsername && password == envPassword);
    }

    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return false;

        return await Task.FromResult(httpContext.User.Identity?.IsAuthenticated ?? false);
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignOutAsync("Cookies");
        }
    }
}
