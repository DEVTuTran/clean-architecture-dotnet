using MediatR;

namespace CleanArchitecture.Application.Commands.Auth;

public class RevokeTokensCommand : IRequest<bool>
{
    public string Uid { get; set; } = string.Empty;
} 