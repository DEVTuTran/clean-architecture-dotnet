using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Login with Firebase ID token
    /// </summary>
    /// <param name="request">Firebase login request</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("firebase-login")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> FirebaseLogin([FromBody] FirebaseLoginRequestDto request)
    {
        try
        {
            var command = new FirebaseLoginCommand
            {
                IdToken = request.IdToken
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized Firebase login attempt");
            return Unauthorized(new { message = "Invalid Firebase token" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Firebase login");
            return BadRequest(new { message = "Login failed" });
        }
    }

    /// <summary>
    /// Register a new user with Firebase
    /// </summary>
    /// <param name="request">Registration request</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var command = new RegisterCommand
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return BadRequest(new { message = "Registration failed" });
        }
    }

    /// <summary>
    /// Refresh authentication token
    /// </summary>
    /// <param name="request">Token refresh request</param>
    /// <returns>New authentication response</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] TokenRefreshRequestDto request)
    {
        try
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = request.RefreshToken
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Invalid refresh token");
            return Unauthorized(new { message = "Invalid refresh token" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return BadRequest(new { message = "Token refresh failed" });
        }
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    /// <returns>Current user information</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(401)]
    public ActionResult<UserDto> GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // You can implement a query to get current user details
            // For now, returning basic user info from claims
            var userDto = new UserDto
            {
                Id = int.Parse(userId),
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "",
                Name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "",
                Uid = User.FindFirst("firebase_uid")?.Value ?? ""
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return BadRequest(new { message = "Failed to get user information" });
        }
    }

    /// <summary>
    /// Logout (client-side token invalidation)
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(200)]
    public ActionResult Logout()
    {
        // In a real implementation, you might want to blacklist the token
        // For now, we'll just return success as token invalidation is typically client-side
        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>
    /// Ban or unban a user
    /// </summary>
    /// <param name="request">Ban user request</param>
    /// <returns>Updated user record</returns>
    [HttpPost("ban-user")]
    [Authorize]
    [ProducesResponseType(typeof(UserRecord), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<UserRecord>> BanUser([FromBody] BanUserRequestDto request)
    {
        try
        {
            var command = new BanUserCommand
            {
                Uid = request.Uid,
                Disabled = request.Disabled
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error banning/unbanning user: {Uid}", request.Uid);
            return BadRequest(new { message = "Failed to ban/unban user" });
        }
    }

    /// <summary>
    /// Ban or unban multiple users
    /// </summary>
    /// <param name="request">Bulk ban request</param>
    /// <returns>Success status</returns>
    [HttpPost("ban-users-bulk")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> BanUsersBulk([FromBody] BanUserBulkRequestDto request)
    {
        try
        {
            var command = new BanUserBulkCommand
            {
                Uids = request.Uids,
                Disabled = request.Disabled
            };

            var result = await _mediator.Send(command);
            return Ok(new { success = result, message = $"Bulk {(request.Disabled ? "ban" : "unban")} completed" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk banning/unbanning users");
            return BadRequest(new { message = "Failed to bulk ban/unban users" });
        }
    }

    /// <summary>
    /// Generate email verification link
    /// </summary>
    /// <param name="request">Email verification request</param>
    /// <returns>Verification link</returns>
    [HttpPost("generate-email-verification")]
    [ProducesResponseType(typeof(EmailVerificationResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<EmailVerificationResponseDto>> GenerateEmailVerification([FromBody] EmailVerificationRequestDto request)
    {
        try
        {
            var command = new GenerateEmailVerificationCommand
            {
                Email = request.Email
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating email verification link for: {Email}", request.Email);
            return BadRequest(new { message = "Failed to generate verification link" });
        }
    }

    /// <summary>
    /// Revoke refresh tokens for a user
    /// </summary>
    /// <param name="request">Revoke tokens request</param>
    /// <returns>Success status</returns>
    [HttpPost("revoke-tokens")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> RevokeTokens([FromBody] RevokeTokensRequestDto request)
    {
        try
        {
            var command = new RevokeTokensCommand
            {
                Uid = request.Uid
            };

            var result = await _mediator.Send(command);
            return Ok(new { success = result, message = "Tokens revoked successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking tokens for user: {Uid}", request.Uid);
            return BadRequest(new { message = "Failed to revoke tokens" });
        }
    }

    /// <summary>
    /// Set user role
    /// </summary>
    /// <param name="request">Set role request</param>
    /// <returns>Success status</returns>
    [HttpPost("set-role")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> SetRole([FromBody] SetRoleRequestDto request)
    {
        try
        {
            var command = new SetRoleCommand
            {
                Uid = request.Uid,
                Role = request.Role,
                CustomClaims = request.CustomClaims
            };

            var result = await _mediator.Send(command);
            return Ok(new { success = result, message = $"Role '{request.Role}' set successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting role '{Role}' for user: {Uid}", request.Role, request.Uid);
            return BadRequest(new { message = "Failed to set role" });
        }
    }
}