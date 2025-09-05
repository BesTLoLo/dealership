using DealershipManagement.Models;

namespace DealershipManagement.Services;

public interface IAuthService
{
    Task<bool> ValidateUserAsync(string username, string password);
    Task<bool> IsUserAuthenticatedAsync();
    Task LogoutAsync();
}
