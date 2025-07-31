using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Mappings;

public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
        CreateMap<Organization, OrganizationDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.PaymentFee, opt => opt.MapFrom(src => src.PaymentFee.Fee));

        CreateMap<CreateOrganizationDto, Organization>();
        CreateMap<UpdateOrganizationDto, Organization>();
    }
} 