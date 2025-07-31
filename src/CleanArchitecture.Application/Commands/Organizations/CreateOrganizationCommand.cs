using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Organizations;

public record CreateOrganizationCommand : IRequest<OrganizationDto>
{
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public int StatusId { get; init; } = 2;
    public int MaxPaymentUsers { get; init; } = 0;
    public int PaymentFeeId { get; init; } = 1;
    public bool UseIpWhitelist { get; init; } = false;
}