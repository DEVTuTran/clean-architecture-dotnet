using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Users;

public record CreateUserCommand : IRequest<UserDto>
{
    public string Uid { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? PasswordHash { get; init; }
    public int PaymentPlanId { get; init; } = 1;
    public int RoleId { get; init; } = 1;
    public bool IsOwner { get; init; } = false;
}