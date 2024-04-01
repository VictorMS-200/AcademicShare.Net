using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicShare.Web.Models;
using AcademicShare.Web.Models.Dtos;
using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace AcademicShare.Web.Profiles;

public class AutoMapperProfiles : Profile
{   
    public AutoMapperProfiles()
    {
        CreateMap<UserProfile, UserProfile>() 
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ReverseMap();

        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
            .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.University))
            .ForMember(dest => dest.Registration, opt => opt.MapFrom(src => src.Registration))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => new UserProfile { Bio = "Hello World!", ProfilePicture = "default.png", Banner = "default.png" }))
            .ReverseMap();

        CreateMap<Post, GetPostDto>()
            .ReverseMap();
    }
}
