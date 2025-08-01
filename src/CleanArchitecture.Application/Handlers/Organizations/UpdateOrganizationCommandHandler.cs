using MediatR;
using AutoMapper;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Commands.Organizations;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Handlers.Organizations;

public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, OrganizationDto>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public UpdateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<OrganizationDto> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(request.Id);

        if (organization == null)
            throw new InvalidOperationException($"Organization with ID {request.Id} not found.");

        organization.Name = request.Name;
        organization.StatusId = request.StatusId;
        organization.MaxPaymentUsers = request.MaxPaymentUsers;
        organization.PaymentFeeId = request.PaymentFeeId;
        organization.UseIpWhitelist = request.UseIpWhitelist;

        var updatedOrganization = await _organizationRepository.UpdateAsync(organization);
        return _mapper.Map<OrganizationDto>(updatedOrganization);
    }
}