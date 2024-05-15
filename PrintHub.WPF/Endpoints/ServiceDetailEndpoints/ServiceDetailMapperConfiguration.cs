using Calabonga.PagedListCore;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;
using PrintHub.WPF.Shared;

namespace PrintHub.WPF.Endpoints.ServiceDetailEndpoints;

public class ServiceDetailMapperConfiguration : Profile
{
    public ServiceDetailMapperConfiguration()
    {
        CreateMap<ServiceDetail, ServiceDetailViewModel>();

        CreateMap<ServiceDetail, Checkable<ServiceDetailViewModel>>()
            .ForMember(checkable => checkable.ViewModel, expression => expression
                .MapFrom((detail, _, _, context) => context.Mapper.Map<ServiceDetailViewModel>(detail)))
            .ForMember(checkable => checkable.IsChecked, expression => expression
                .MapFrom(model => false));

        CreateMap<ServiceDetailViewModel, Checkable<ServiceDetailViewModel>>()
            .ForMember(checkable => checkable.ViewModel, expression => expression
                .MapFrom(model => model))
            .ForMember(checkable => checkable.IsChecked, expression => expression
                .MapFrom(model => false));

        CreateMap<IPagedList<ServiceDetail>, IPagedList<ServiceDetailViewModel>>()
            .ConvertUsing<PagedListConverter<ServiceDetail, ServiceDetailViewModel>>();
    }
}