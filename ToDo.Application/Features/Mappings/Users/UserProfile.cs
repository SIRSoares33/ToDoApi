using AutoMapper;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Features.Mappings.Users;

/// <summary>
/// Provides AutoMapper profile configuration for mapping between user-related data transfer objects and domain models.
/// </summary>
/// <remarks>This profile defines mappings between RegisterDto and User, as well as between lists of User and
/// UserDto. It is intended to be used with AutoMapper to facilitate object-object mapping in user registration and
/// retrieval scenarios.</remarks>
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, User>()
            .ForPath(
                dest => dest.Email.Value,
                opt => opt.MapFrom(src => src.Email)
            )
            .ForPath(
                dest => dest.Name.Value,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForPath(
                dest => dest.HashPassword.Value,
                opt => opt.Ignore()
            )
            .ForPath(dest => dest.Role, opt => opt.Ignore()).ReverseMap();

        CreateMap<User, UserDto>()
        .ForPath(dest => dest.Id,
            opt => opt.MapFrom(src => src.Id))
        .ForPath(dest => dest.Name,
            opt => opt.MapFrom(src => src.Name.Value))
        .ForPath(dest => dest.Email,
            opt => opt.MapFrom(src => src.Email.Value))
            .ReverseMap();

        CreateMap<UpdateUserDto, User>()
            .ForPath(dest => dest.Email.Value,
                opt => opt.MapFrom(src => src.Email))
            .ForPath(dest => dest.Name.Value,
                opt => opt.MapFrom(src => src.Name))
            .ForPath(dest => dest.HashPassword.Value,
                opt => opt.Ignore())
            .ForPath(dest => dest.Role, opt => opt.Ignore()).ReverseMap();

        CreateMap<UpdateUserByAdminDto, User>()
            .ForPath(dest => dest.Email.Value,
                opt => opt.MapFrom(src => src.Email))
            .ForPath(dest => dest.Name.Value,
                opt => opt.MapFrom(src => src.Name))
            .ForPath(dest => dest.HashPassword.Value,
                opt => opt.Ignore())
            .ForPath(dest => dest.Role, opt => opt.MapFrom(src => src.Role)).ReverseMap();
    }
}