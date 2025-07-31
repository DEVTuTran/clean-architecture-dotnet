using MediatR;
using AutoMapper;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Queries.Organizations;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Features.Organization.Handlers;

public class GetAllOrganizationsQueryHandler : IRequestHandler<GetAllOrganizationsQuery, IEnumerable<OrganizationDto>>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public GetAllOrganizationsQueryHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrganizationDto>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var organizations = await _organizationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
    }
}