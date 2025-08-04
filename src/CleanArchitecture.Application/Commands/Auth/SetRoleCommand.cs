using MediatR;

namespace CleanArchitecture.Application.Commands.Auth;

public class SetRoleCommand : IRequest<bool>
{
    public string Uid { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public Dictionary<string, object>? CustomClaims { get; set; }
} 