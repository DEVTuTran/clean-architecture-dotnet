using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Handlers.Auth;

public class RevokeTokensCommandHandler : IRequestHandler<RevokeTokensCommand, bool>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly ILogger<RevokeTokensCommandHandler> _logger;

    public RevokeTokensCommandHandler(
        IFirebaseAuthService firebaseAuthService,
        ILogger<RevokeTokensCommandHandler> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _logger = logger;
    }

    public async Task<bool> Handle(RevokeTokensCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _firebaseAuthService.RevokeRefreshTokensAsync(request.Uid);
            
            _logger.LogInformation("Refresh tokens revoked for user: {Uid}", request.Uid);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking refresh tokens for user: {Uid}", request.Uid);
            throw;
        }
    }
} 