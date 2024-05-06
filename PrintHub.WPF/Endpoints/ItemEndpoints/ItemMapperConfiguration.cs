using AutoMapper;
using Calabonga.PagedListCore;
using PrintHub.Domain;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints;

public class ItemMapperConfiguration : Profile
{
    public ItemMapperConfiguration()
    {
        CreateMap<ItemCreateViewModel, Item>()
            .ForMember(order => order.CreatedAt, expression => expression.Ignore())
            .ForMember(order => order.CreatedBy, expression => expression.MapFrom((_, _, _, context) => context.Items[nameof(ApplicationUser)]))
            .ForMember(order => order.UpdatedAt, expression => expression.Ignore())
            .ForMember(order => order.UpdatedBy, expression => expression.Ignore())
            .ForMember(order => order.Ready, expression => expression.Ignore())
            .ForMember(order => order.Order, expression => expression.Ignore())
            .ForMember(order => order.PrintingDetails, expression => expression.Ignore())
            ;

        CreateMap<Item, ItemViewModel>();

        CreateMap<IPagedList<Item>, IPagedList<ItemViewModel>>()
            .ConvertUsing<PagedListConverter<Item, ItemViewModel>>();
    }
}