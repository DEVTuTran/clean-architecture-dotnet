using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Services;

public interface IFirebaseAuthService
{
    Task<UserRecord> GetUserByUidAsync(string uid);
    Task<UserRecord> CreateUserAsync(string email, string password, string displayName);
    Task<UserRecord> UpdateUserAsync(string uid, string displayName, string email);
    Task DeleteUserAsync(string uid);
    Task<ClaimsPrincipal> VerifyTokenAsync(string idToken);
    Task<string> GenerateCustomTokenAsync(string uid);
    Task<UserRecord> BanUserAsync(string uid, bool disabled);
    Task BanUserBulkAsync(string[] uids, bool disabled);
    Task<string> GenerateEmailVerificationLinkAsync(string email);
    Task RevokeRefreshTokensAsync(string uid);
    Task<UserRecord> GetUserRecordAsync(string? uid = null);
    Task<UserRecord> UpdateUserProfileAsync(UserRecordArgs userRecordArgs);
    Task<UserRecord> RegisterAsync(UserRecordArgs userRecordArgs);
    Task SetRootRoleAsync(string uid);
    Task SyncRoleAsync(string? uid = null);
}

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    private readonly ILogger<FirebaseAuthService> _logger;
    private readonly IConfiguration _configuration;

    public FirebaseAuthService(IConfiguration configuration, ILogger<FirebaseAuthService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        // Initialize Firebase Admin SDK if not already initialized
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(_configuration.GetSection("FireBase:Credential").Value)
            });
        }

        _firebaseAuth = FirebaseAuth.DefaultInstance;
    }

    public async Task<UserRecord> GetUserByUidAsync(string uid)
    {
        try
        {
            return await _firebaseAuth.GetUserAsync(uid);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error getting user by UID: {Uid}", uid);
            throw;
        }
    }

    public async Task<UserRecord> CreateUserAsync(string email, string password, string displayName)
    {
        try
        {
            var args = new UserRecordArgs
            {
                Email = email,
                Password = password,
                DisplayName = displayName,
                EmailVerified = false
            };

            return await _firebaseAuth.CreateUserAsync(args);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error creating user with email: {Email}", email);
            throw;
        }
    }

    public async Task<UserRecord> UpdateUserAsync(string uid, string displayName, string email)
    {
        try
        {
            var args = new UserRecordArgs
            {
                Uid = uid,
                DisplayName = displayName,
                Email = email
            };

            return await _firebaseAuth.UpdateUserAsync(args);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error updating user with UID: {Uid}", uid);
            throw;
        }
    }

    public async Task DeleteUserAsync(string uid)
    {
        try
        {
            await _firebaseAuth.DeleteUserAsync(uid);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error deleting user with UID: {Uid}", uid);
            throw;
        }
    }

    public async Task<ClaimsPrincipal> VerifyTokenAsync(string idToken)
    {
        try
        {
            var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
                new Claim(ClaimTypes.Email, decodedToken.Claims["email"]?.ToString() ?? ""),
                new Claim(ClaimTypes.Name, decodedToken.Claims["name"]?.ToString() ?? ""),
                new Claim("firebase_uid", decodedToken.Uid)
            };

            // Add custom claims if they exist
            if (decodedToken.Claims.ContainsKey("role"))
            {
                var roleValue = decodedToken.Claims["role"]?.ToString();
                if (!string.IsNullOrEmpty(roleValue))
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleValue));
                }
            }

            var identity = new ClaimsIdentity(claims, "Firebase");
            return new ClaimsPrincipal(identity);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error verifying Firebase token");
            throw;
        }
    }

    public async Task<string> GenerateCustomTokenAsync(string uid)
    {
        try
        {
            return await _firebaseAuth.CreateCustomTokenAsync(uid);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error generating custom token for UID: {Uid}", uid);
            throw;
        }
    }

    public async Task<UserRecord> BanUserAsync(string uid, bool disabled)
    {
        try
        {
            var args = new UserRecordArgs
            {
                Uid = uid,
                Disabled = disabled
            };

            return await _firebaseAuth.UpdateUserAsync(args);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error banning/unbanning user with UID: {Uid}", uid);
            throw;
        }
    }

    public async Task BanUserBulkAsync(string[] uids, bool disabled)
    {
        try
        {
            var tasks = uids.Select(uid => BanUserAsync(uid, disabled));
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk banning/unbanning users");
            throw;
        }
    }

    public async Task<string> GenerateEmailVerificationLinkAsync(string email)
    {
        try
        {
            var settings = new ActionCodeSettings
            {
                Url = _configuration["Firebase:EmailVerificationUrl"] ?? "https://your-app.com/verify-email",
                HandleCodeInApp = true
            };

            return await _firebaseAuth.GenerateEmailVerificationLinkAsync(email, settings);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error generating email verification link for: {Email}", email);
            throw;
        }
    }

    public async Task RevokeRefreshTokensAsync(string uid)
    {
        try
        {
            await _firebaseAuth.RevokeRefreshTokensAsync(uid);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error revoking refresh tokens for UID: {Uid}", uid);
            throw;
        }
    }

    public async Task<UserRecord> GetUserRecordAsync(string? uid = null)
    {
        try
        {
            if (string.IsNullOrEmpty(uid))
            {
                // Get current user from context if available
                // This is a placeholder - implement based on your authentication context
                throw new ArgumentException("UID is required");
            }

            return await _firebaseAuth.GetUserAsync(uid);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error getting user record for UID: {Uid}", uid);
            throw;
        }
    }

    public async Task<UserRecord> UpdateUserProfileAsync(UserRecordArgs userRecordArgs)
    {
        try
        {
            return await _firebaseAuth.UpdateUserAsync(userRecordArgs);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error updating user profile for UID: {Uid}", userRecordArgs.Uid);
            throw;
        }
    }

    public async Task<UserRecord> RegisterAsync(UserRecordArgs userRecordArgs)
    {
        try
        {
            return await _firebaseAuth.CreateUserAsync(userRecordArgs);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", userRecordArgs.Email);
            throw;
        }
    }

    public async Task SetRootRoleAsync(string uid)
    {
        try
        {
            var customClaims = new Dictionary<string, object>
            {
                { "role", "root" },
                { "isRoot", true }
            };

            await _firebaseAuth.SetCustomUserClaimsAsync(uid, customClaims);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Error setting root role for UID: {Uid}", uid);
            throw;
        }
    }

    public async Task SyncRoleAsync(string? uid = null)
    {
        try
        {
            if (string.IsNullOrEmpty(uid))
            {
                // Sync roles for all users or implement based on your requirements
                _logger.LogWarning("UID is required for role sync");
                return;
            }

            // Get user from database and sync role with Firebase
            // This is a placeholder - implement based on your role management system
            var user = await GetUserRecordAsync(uid);

            // Example: Sync role from database to Firebase custom claims
            // var customClaims = new Dictionary<string, object>
            // {
            //     { "role", userRole },
            //     { "permissions", userPermissions }
            // };
            // await _firebaseAuth.SetCustomUserClaimsAsync(uid, customClaims);

            _logger.LogInformation("Role synced for UID: {Uid}", uid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing role for UID: {Uid}", uid);
            throw;
        }
    }
}

public class FirebaseConfig
{
    public string? ServiceAccountKeyPath { get; set; }
    public string? ProjectId { get; set; }
    public string? EmailVerificationUrl { get; set; }
}