using System.Runtime.CompilerServices;
using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Shared;

namespace Services.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            //source - > destination
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
            

            CreateMap<TeamMember, TeamMemberUpdateDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName));
            CreateMap<TeamMemberUpdateDto, TeamMember>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status { StatusName = src.Status }));
            CreateMap<TeamMember, TeamMemberResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName));


            CreateMap<TeamMember, TeamMemberCreateDto>();
            CreateMap<TeamMemberCreateDto, TeamMember>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status { StatusName = src.Status}));

            CreateMap<Client, ClientCreateDto>().ReverseMap();
            CreateMap<Client, ClientUpdateDto>().ReverseMap();
            CreateMap<Client, ClientResponse>();

            CreateMap<ProjectCreateDto, Project>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status { StatusName = "Inactive" }));

            CreateMap<ProjectUpdateDto, Project>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status { StatusName = src.Status }));
            CreateMap<Project, ProjectUpdateDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName));
            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Lead, opt => opt.MapFrom(src => src.Lead.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName));

            CreateMap<ActivityCreateDto, Activity>();
            CreateMap<ActivityUpdateDto, Activity>();
            CreateMap<Activity, ActivityResponse>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

        }
    }
}
