using AutoMapper;
using Calabonga.PagedListCore;
using PrintHub.Domain;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ServiceDetailEndpoints;

public class ServiceDetailMapperConfiguration : Profile
{
    public ServiceDetailMapperConfiguration()
    {
        CreateMap<ServiceDetail, ServiceDetailViewModel>();

        CreateMap<IPagedList<ServiceDetail>, IPagedList<ServiceDetailViewModel>>()
            .ConvertUsing<PagedListConverter<ServiceDetail, ServiceDetailViewModel>>();
    }
}