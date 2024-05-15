using Calabonga.PagedListCore;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints;

public class AdditionalServiceMapperConfiguration : Profile
{
    public AdditionalServiceMapperConfiguration()
    {
        CreateMap<AdditionalService, AdditionalServiceViewModel>();
        CreateMap<AdditionalService, AdditionalServiceSelectViewModel>();
        CreateMap<AdditionalServiceViewModel, AdditionalServiceSelectViewModel>();

        CreateMap<IPagedList<AdditionalService>, IPagedList<AdditionalServiceViewModel>>()
            .ConvertUsing<PagedListConverter<AdditionalService, AdditionalServiceViewModel>>();
    }
}