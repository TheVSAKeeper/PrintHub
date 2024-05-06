using AutoMapper;
using Calabonga.PagedListCore;
using PrintHub.Domain;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints;

public class PrintingDetailsMapperConfiguration : Profile
{
    public PrintingDetailsMapperConfiguration()
    {
        CreateMap<PrintingDetailsCreateViewModel, PrintingDetails>()
            .ForMember(details => details.Samples, expression => expression.Ignore())
            .ForMember(details => details.Items, expression => expression.Ignore())
            .ForMember(details => details.Color, expression => expression.Ignore())
            .ForMember(details => details.Material, expression => expression.Ignore())
            ;

        CreateMap<PrintingDetails, PrintingDetailsViewModel>()
            .ForMember(viewModel => viewModel.ColorViewModel,
                expression => expression.MapFrom(details => details.Color))
            .ForMember(viewModel => viewModel.MaterialViewModel,
                expression => expression.MapFrom(details => details.Material));

        CreateMap<IPagedList<PrintingDetails>, IPagedList<PrintingDetailsViewModel>>()
            .ConvertUsing<PagedListConverter<PrintingDetails, PrintingDetailsViewModel>>();
    }
}