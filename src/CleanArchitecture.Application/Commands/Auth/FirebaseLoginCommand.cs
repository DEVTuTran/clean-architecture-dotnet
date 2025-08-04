using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Auth;

public class FirebaseLoginCommand : IRequest<AuthResponseDto>
{
    public string IdToken { get; set; } = string.Empty;
} 