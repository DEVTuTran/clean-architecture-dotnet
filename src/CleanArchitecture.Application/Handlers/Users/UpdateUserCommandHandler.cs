using MediatR;
using AutoMapper;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Commands.Users;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Handlers.Users;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
            throw new Exception("User not found");

        user.Name = request.Name ?? user.Name;
        user.PasswordHash = request.PasswordHash ?? user.PasswordHash;
        user.PaymentPlanId = request.PaymentPlanId;
        user.RoleId = request.RoleId;
        user.IsDisabled = request.IsDisabled;
        user.MustChangePassword = request.MustChangePassword;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(updatedUser);
    }
}