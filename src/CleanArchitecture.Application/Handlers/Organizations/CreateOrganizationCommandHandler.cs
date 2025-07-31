using MediatR;
using AutoMapper;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Commands.Organizations;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Handlers.Organizations;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, OrganizationDto>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public CreateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<OrganizationDto> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = new Organization
        {
            Name = request.Name,
            Code = request.Code,
            StatusId = request.StatusId,
            MaxPaymentUsers = request.MaxPaymentUsers,
            PaymentFeeId = request.PaymentFeeId,
            UseIpWhitelist = request.UseIpWhitelist
        };

        var createdOrganization = await _organizationRepository.AddAsync(organization);
        return _mapper.Map<OrganizationDto>(createdOrganization);
    }
}