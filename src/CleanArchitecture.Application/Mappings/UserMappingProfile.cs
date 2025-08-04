using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.PaymentPlanName, opt => opt.MapFrom(src => src.PaymentPlan.Name))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }

    public static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Uid = user.Uid,
            Email = user.Email,
            Name = user.Name ?? "",
            EmailVerifiedAt = user.EmailVerifiedAt,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name ?? "",
            PaymentPlanId = user.PaymentPlanId,
            PaymentPlanName = user.PaymentPlan?.Name ?? "",
            IsDisabled = user.IsDisabled,
            ClientsCount = user.ClientsCount,
            IsOwner = user.IsOwner,
            LastLoginAt = user.LastLoginAt,
            LastLoginIp = user.LastLoginIp,
            MustChangePassword = user.MustChangePassword,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}