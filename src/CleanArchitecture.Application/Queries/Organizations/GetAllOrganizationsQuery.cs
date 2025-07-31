using MediatR;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Queries.Organizations;

public record GetAllOrganizationsQuery : IRequest<IEnumerable<OrganizationDto>>;