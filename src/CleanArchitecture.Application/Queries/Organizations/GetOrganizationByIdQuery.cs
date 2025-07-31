using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Queries.Organizations;

public record GetOrganizationByIdQuery : IRequest<OrganizationDto?>
{
    public int Id { get; init; }
}