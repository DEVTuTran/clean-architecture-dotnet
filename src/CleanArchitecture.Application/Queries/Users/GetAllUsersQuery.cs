using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Queries.Users;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;