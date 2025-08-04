using MediatR;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Commands.Users;

namespace CleanArchitecture.Application.Handlers.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
            return false;

        await _userRepository.DeleteAsync(request.Id);
        return true;
    }
}