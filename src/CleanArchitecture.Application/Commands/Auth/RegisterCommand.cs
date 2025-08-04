using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Auth;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
} 