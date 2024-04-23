using AutoMapper;
using PrintHub.Domain;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Views;

namespace PrintHub.WPF.Mappers;

public class PrintingDetailsMapperConfiguration : Profile
{
    public PrintingDetailsMapperConfiguration()
    {
        CreateMap<PrintingDetails, PrintingDetailsViewModel>()
            .ForMember(dest => dest.ColorViewModel,
                opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.MaterialViewModel,
                opt => opt.MapFrom(src => src.Material));
    }
}