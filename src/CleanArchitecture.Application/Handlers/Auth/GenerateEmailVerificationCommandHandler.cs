using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Handlers.Auth;

public class GenerateEmailVerificationCommandHandler : IRequestHandler<GenerateEmailVerificationCommand, EmailVerificationResponseDto>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly ILogger<GenerateEmailVerificationCommandHandler> _logger;

    public GenerateEmailVerificationCommandHandler(
        IFirebaseAuthService firebaseAuthService,
        ILogger<GenerateEmailVerificationCommandHandler> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _logger = logger;
    }

    public async Task<EmailVerificationResponseDto> Handle(GenerateEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var verificationLink = await _firebaseAuthService.GenerateEmailVerificationLinkAsync(request.Email);
            
            _logger.LogInformation("Email verification link generated for: {Email}", request.Email);
            
            return new EmailVerificationResponseDto
            {
                VerificationLink = verificationLink,
                ExpiresAt = DateTime.UtcNow.AddHours(24) // Firebase links typically expire in 24 hours
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating email verification link for: {Email}", request.Email);
            throw;
        }
    }
} 