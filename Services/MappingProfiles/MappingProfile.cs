using AutoMapper;
using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            //source - > destination
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            
            CreateMap<TeamMember, TeamMemberDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName));
            CreateMap<TeamMemberDto, TeamMember>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status { StatusName = src.Status }));


            CreateMap<TeamMember, CreateTeamMemberDto>();
            CreateMap<CreateTeamMemberDto, TeamMember>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status { StatusName = src.Status}));

            
        }
    }
}
