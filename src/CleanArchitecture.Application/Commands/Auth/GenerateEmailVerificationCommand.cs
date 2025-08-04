using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Auth;

public class GenerateEmailVerificationCommand : IRequest<EmailVerificationResponseDto>
{
    public string Email { get; set; } = string.Empty;
} 