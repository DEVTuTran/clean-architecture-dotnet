using CleanArchitecture.Application.Commands.Auth;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Handlers.Auth;

public class BanUserBulkCommandHandler : IRequestHandler<BanUserBulkCommand, bool>
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly ILogger<BanUserBulkCommandHandler> _logger;

    public BanUserBulkCommandHandler(
        IFirebaseAuthService firebaseAuthService,
        ILogger<BanUserBulkCommandHandler> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _logger = logger;
    }

    public async Task<bool> Handle(BanUserBulkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _firebaseAuthService.BanUserBulkAsync(request.Uids, request.Disabled);
            
            _logger.LogInformation("Bulk {Action} completed for {Count} users", 
                request.Disabled ? "ban" : "unban", 
                request.Uids.Length);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk banning/unbanning users");
            throw;
        }
    }
} 