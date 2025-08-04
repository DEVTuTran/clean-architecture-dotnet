using CleanArchitecture.Infrastructure.Services;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace CleanArchitecture.API.Middleware;

public class FirebaseAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FirebaseAuthMiddleware> _logger;

    public FirebaseAuthMiddleware(RequestDelegate next, ILogger<FirebaseAuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IFirebaseAuthService firebaseAuthService)
    {
        try
        {
            // Check if the request has a Firebase ID token
            if (context.Request.Headers.TryGetValue("X-Firebase-ID-Token", out StringValues firebaseToken))
            {
                var token = firebaseToken.ToString();
                if (!string.IsNullOrEmpty(token))
                {
                    // Verify the Firebase token
                    var claimsPrincipal = await firebaseAuthService.VerifyTokenAsync(token);

                    // Set the user principal for the request
                    context.User = claimsPrincipal;

                    _logger.LogInformation("Firebase token verified for user: {UserId}",
                        claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to verify Firebase token");
            // Don't throw here - let the request continue without authentication
        }

        await _next(context);
    }
}

// Extension method for easy middleware registration
public static class FirebaseAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseFirebaseAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FirebaseAuthMiddleware>();
    }
}