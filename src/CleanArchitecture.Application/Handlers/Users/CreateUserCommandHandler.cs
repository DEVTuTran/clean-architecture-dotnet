using MediatR;
using AutoMapper;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Application.Commands.Users;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Handlers.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Uid = request.Uid,
            Name = request.Name,
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            PaymentPlanId = request.PaymentPlanId,
            RoleId = request.RoleId,
            IsOwner = request.IsOwner
        };

        var createdUser = await _userRepository.AddAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }
}