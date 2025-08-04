using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Auth;

public class RefreshTokenCommand : IRequest<AuthResponseDto>
{
    public string RefreshToken { get; set; } = string.Empty;
} 