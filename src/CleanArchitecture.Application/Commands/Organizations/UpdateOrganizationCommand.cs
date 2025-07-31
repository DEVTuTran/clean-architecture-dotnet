using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Commands.Organizations;

public record UpdateOrganizationCommand : IRequest<OrganizationDto>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int StatusId { get; init; }
    public int MaxPaymentUsers { get; init; }
    public int PaymentFeeId { get; init; }
    public bool UseIpWhitelist { get; init; }
}