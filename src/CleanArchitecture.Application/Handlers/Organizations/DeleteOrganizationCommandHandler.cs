using MediatR;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Commands.Organizations;

namespace CleanArchitecture.Application.Handlers.Organizations;

public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, bool>
{
    private readonly IOrganizationRepository _organizationRepository;

    public DeleteOrganizationCommandHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<bool> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(request.Id);

        if (organization == null)
            return false;

        await _organizationRepository.DeleteAsync(request.Id);
        return true;
    }
}