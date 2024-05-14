using AutoMapper;
using Calabonga.PagedListCore;
using PrintHub.Domain;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints;

public class AdditionalServiceMapperConfiguration : Profile
{
    public AdditionalServiceMapperConfiguration()
    {
        CreateMap<AdditionalService, AdditionalServiceViewModel>();

        CreateMap<IPagedList<AdditionalService>, IPagedList<AdditionalServiceViewModel>>()
            .ConvertUsing<PagedListConverter<AdditionalService, AdditionalServiceViewModel>>();
    }
}