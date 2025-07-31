using MediatR;
using AutoMapper;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Queries.Organizations;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Handlers.Organizations;

public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto?>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public GetOrganizationByIdQueryHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<OrganizationDto?> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(request.Id);
        return organization != null ? _mapper.Map<OrganizationDto>(organization) : null;
    }
}