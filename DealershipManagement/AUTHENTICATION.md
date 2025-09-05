# Authentication Setup

This application now includes basic username/password authentication using environment variables.

## Environment Variables

Set the following environment variables for authentication:

### Required Environment Variables:
- `AUTH_USERNAME` - The username for login
- `AUTH_PASSWORD` - The password for login

### Example:
```bash
# Windows (PowerShell)
$env:AUTH_USERNAME="admin"
$env:AUTH_PASSWORD="your_secure_password"

# Windows (Command Prompt)
set AUTH_USERNAME=admin
set AUTH_PASSWORD=your_secure_password

# Linux/macOS
export AUTH_USERNAME="admin"
export AUTH_PASSWORD="your_secure_password"
```

## Configuration File

You can also set default credentials in `appsettings.json`:

```json
{
  "Authentication": {
    "Username": "admin",
    "Password": "password123"
  }
}
```

**Note:** Environment variables take precedence over configuration file values.

## Features

- **Login Page**: Navigate to `/login` to access the login form
- **Logout**: Use the logout link in the navigation menu or navigate to `/logout`
- **Protected Pages**: The Cars page now requires authentication
- **Session Management**: Authentication cookies expire after 1 hour with sliding expiration
- **Navigation**: Login/Logout links appear in the navigation menu based on authentication status

## Security Notes

- This is a basic implementation for demonstration purposes
- In production, consider:
  - Using proper password hashing (bcrypt, Argon2, etc.)
  - Implementing user roles and permissions
  - Adding two-factor authentication
  - Using HTTPS in production
  - Implementing proper session management
  - Adding rate limiting for login attempts

## Usage

1. Set the environment variables or update the configuration file
2. Start the application
3. Navigate to any protected page (like `/cars`) - you'll be redirected to login
4. Enter your credentials to access the application
5. Use the logout link when done
