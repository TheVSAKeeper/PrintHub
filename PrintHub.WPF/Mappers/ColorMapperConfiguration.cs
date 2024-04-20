using AutoMapper;
using PrintHub.Domain;
using PrintHub.WPF.Endpoints.ColorEndpoints;

namespace PrintHub.WPF.Mappers;

public class ColorMapperConfiguration : Profile
{
    public ColorMapperConfiguration()
    {
        CreateMap<Color, ColorViewModel>();
    }
}