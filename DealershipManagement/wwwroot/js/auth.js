// Authentication cookie management
window.auth = {
    createAuthCookie: function(username) {
        // Create a simple authentication cookie
        document.cookie = `auth_user=${username}; path=/; max-age=3600; secure; samesite=strict`;
    },
    
    clearAuthCookie: function() {
        // Clear the authentication cookie
        document.cookie = 'auth_user=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT';
    },
    
    getAuthCookie: function() {
        // Get the authentication cookie value
        const name = 'auth_user=';
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return '';
    }
};
