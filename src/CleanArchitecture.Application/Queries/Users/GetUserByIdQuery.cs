using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Queries.Users;

public record GetUserByIdQuery : IRequest<UserDto?>
{
    public int Id { get; init; }
}