using AutoMapper;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Features.Mappings.ToDo;

public class ToDoProfile : Profile
{
    public ToDoProfile()
    {
        CreateMap<AddToDoDto, Todo>()
        .ForPath(dest => dest.Title.Value,
            opt => opt.MapFrom(src => src.Title))
        .ForPath(dest => dest.Description.Value,
            opt => opt.MapFrom(src => src.Description))
        .ForMember(dest => dest.IsCompleted,
            opt => opt.MapFrom(src => src.IsCompleted))
        .ForMember(dest => dest.CreateAt,
            opt => opt.MapFrom(_ => DateTime.UtcNow))
        .ForMember(dest => dest.UpdateAt,
            opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Todo, ToDoDto>()
            .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForPath(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdateAt))
            .ReverseMap();
    }
}
