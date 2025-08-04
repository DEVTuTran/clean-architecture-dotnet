using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Test endpoint that requires authentication
    /// </summary>
    /// <returns>User information from claims</returns>
    [HttpGet("authenticated")]
    [Authorize]
    public ActionResult<object> GetAuthenticatedUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var firebaseUid = User.FindFirst("firebase_uid")?.Value;

        return Ok(new
        {
            message = "Authentication successful!",
            user = new
            {
                userId,
                email,
                name,
                firebaseUid,
                authenticationType = User.Identity?.AuthenticationType
            }
        });
    }

    /// <summary>
    /// Test endpoint that doesn't require authentication
    /// </summary>
    /// <returns>Public message</returns>
    [HttpGet("public")]
    public ActionResult<object> GetPublicMessage()
    {
        return Ok(new
        {
            message = "This is a public endpoint",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Test endpoint to check if user is authenticated (optional)
    /// </summary>
    /// <returns>User information if authenticated</returns>
    [HttpGet("optional")]
    public ActionResult<object> GetOptionalAuth()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                message = "User is authenticated",
                user = new
                {
                    userId,
                    email
                }
            });
        }

        return Ok(new
        {
            message = "User is not authenticated",
            user = (object?)null
        });
    }
}