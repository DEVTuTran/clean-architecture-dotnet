using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Application.Handlers.Auth;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // In a real implementation, you would validate the refresh token against a database
            // For now, we'll just generate a new token (you should implement proper refresh token validation)

            // Extract user ID from refresh token (this is a simplified approach)
            // In production, you should store refresh tokens in a database with user associations
            var userId = ExtractUserIdFromRefreshToken(request.RefreshToken);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            // Generate new JWT token
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Map to DTO
            var userDto = UserMappingProfile.MapToDto(user);

            return new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = userDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            throw;
        }
    }

    private int? ExtractUserIdFromRefreshToken(string refreshToken)
    {
        // This is a simplified implementation
        // In production, you should implement proper refresh token validation
        // For now, we'll assume the refresh token contains the user ID in some form
        try
        {
            // This is just a placeholder - implement proper token validation
            if (Guid.TryParse(refreshToken, out _))
            {
                // Return a default user ID for demonstration
                // In real implementation, you'd decode the refresh token properly
                return 1;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"] ?? "your-secret-key-here-minimum-16-characters";
        var issuer = jwtSettings["Issuer"] ?? "CleanArchitecture";
        var audience = jwtSettings["Audience"] ?? "CleanArchitecture";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name ?? ""),
            new Claim("firebase_uid", user.Uid),
            new Claim("role_id", user.RoleId.ToString())
        };

        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}