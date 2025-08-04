using MediatR;

namespace CleanArchitecture.Application.Commands.Users;

public record DeleteUserCommand : IRequest<bool>
{
    public int Id { get; init; }
}