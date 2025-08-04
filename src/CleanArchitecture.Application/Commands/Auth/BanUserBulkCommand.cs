using MediatR;

namespace CleanArchitecture.Application.Commands.Auth;

public class BanUserBulkCommand : IRequest<bool>
{
    public string[] Uids { get; set; } = Array.Empty<string>();
    public bool Disabled { get; set; } = true;
} 