using MediatR;

namespace CleanArchitecture.Application.Commands.Organizations;

public record DeleteOrganizationCommand : IRequest<bool>
{
    public int Id { get; init; }
}