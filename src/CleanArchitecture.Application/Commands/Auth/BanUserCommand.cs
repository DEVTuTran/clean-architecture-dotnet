using MediatR;
using FirebaseAdmin.Auth;

namespace CleanArchitecture.Application.Commands.Auth;

public class BanUserCommand : IRequest<UserRecord>
{
    public string Uid { get; set; } = string.Empty;
    public bool Disabled { get; set; } = true;
} 