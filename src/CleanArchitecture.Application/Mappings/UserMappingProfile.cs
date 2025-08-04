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
}