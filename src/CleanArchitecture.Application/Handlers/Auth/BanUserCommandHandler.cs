using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using FirebaseAdmin.Auth;

namespace CleanArchitecture.Application.Handlers.Auth;

public class BanUserCommandHandler : IRequestHandler<BanUserCommand, UserRecord>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly ILogger<BanUserCommandHandler> _logger;

    public BanUserCommandHandler(
        IFirebaseAuthService firebaseAuthService,
        ILogger<BanUserCommandHandler> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _logger = logger;
    }

    public async Task<UserRecord> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _firebaseAuthService.BanUserAsync(request.Uid, request.Disabled);
            
            _logger.LogInformation("User {Uid} {Action}", 
                request.Uid, 
                request.Disabled ? "banned" : "unbanned");
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error banning/unbanning user {Uid}", request.Uid);
            throw;
        }
    }
} 