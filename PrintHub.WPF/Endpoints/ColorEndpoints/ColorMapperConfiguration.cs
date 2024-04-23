using AutoMapper;
using Calabonga.PagedListCore;
using PrintHub.Domain;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints;

public class ColorMapperConfiguration : Profile
{
    public ColorMapperConfiguration()
    {
        CreateMap<ColorCreateViewModel, Color>()
            .ForMember(color => color.Materials, expression => expression.Ignore())
            .ForMember(color => color.Orders, expression => expression.Ignore())
            .ForMember(color => color.PrintingDetails, expression => expression.Ignore())
            ;

        CreateMap<Color, ColorViewModel>();

        CreateMap<Color, ColorUpdateViewModel>();

        CreateMap<ColorUpdateViewModel, Color>()
            .ForMember(color => color.Materials, expression => expression.Ignore())
            .ForMember(color => color.Orders, expression => expression.Ignore())
            .ForMember(color => color.PrintingDetails, expression => expression.Ignore())
            ;

        CreateMap<IPagedList<Color>, IPagedList<ColorViewModel>>()
            .ConvertUsing<PagedListConverter<Color, ColorViewModel>>();
    }
}