using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Application.Handlers.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IFirebaseAuthService firebaseAuthService,
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<RegisterCommandHandler> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists in database
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            // Create user in Firebase
            var firebaseUser = await _firebaseAuthService.CreateUserAsync(
                request.Email, 
                request.Password, 
                request.Name
            );

            // Create user in database
            var user = new User
            {
                Uid = firebaseUser.Uid,
                Email = request.Email,
                Name = request.Name,
                EmailVerifiedAt = firebaseUser.EmailVerified ? DateTime.UtcNow : null,
                LastLoginAt = DateTime.UtcNow,
                RoleId = 1, // Default role
                PaymentPlanId = 1 // Default payment plan
            };

            await _userRepository.AddAsync(user);

            // Generate JWT token
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
            _logger.LogError(ex, "Error during user registration");
            throw;
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