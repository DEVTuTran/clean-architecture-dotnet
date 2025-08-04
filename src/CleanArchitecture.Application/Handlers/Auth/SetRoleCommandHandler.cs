using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Handlers.Auth;

public class SetRoleCommandHandler : IRequestHandler<SetRoleCommand, bool>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly ILogger<SetRoleCommandHandler> _logger;

    public SetRoleCommandHandler(
        IFirebaseAuthService firebaseAuthService,
        ILogger<SetRoleCommandHandler> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _logger = logger;
    }

    public async Task<bool> Handle(SetRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Role.ToLower() == "root")
            {
                await _firebaseAuthService.SetRootRoleAsync(request.Uid);
            }
            else
            {
                // For other roles, you can implement custom logic
                // This is a placeholder - implement based on your role management system
                _logger.LogWarning("Role '{Role}' not implemented yet", request.Role);
                return false;
            }
            
            _logger.LogInformation("Role '{Role}' set for user: {Uid}", request.Role, request.Uid);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting role '{Role}' for user: {Uid}", request.Role, request.Uid);
            throw;
        }
    }
} 