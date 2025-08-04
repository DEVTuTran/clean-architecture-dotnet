using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Users;

public record UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? PasswordHash { get; init; }
    public int PaymentPlanId { get; init; }
    public int RoleId { get; init; }
    public bool IsDisabled { get; init; }
    public bool MustChangePassword { get; init; }
}