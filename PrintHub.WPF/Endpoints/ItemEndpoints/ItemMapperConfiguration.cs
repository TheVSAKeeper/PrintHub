using Calabonga.PagedListCore;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints;

public class ItemMapperConfiguration : Profile
{
    public ItemMapperConfiguration()
    {
        CreateMap<ItemCreateViewModel, Item>()
            .ForMember(item => item.CreatedAt, expression => expression.Ignore())
            .ForMember(item => item.CreatedBy, expression => expression
                .MapFrom((_, _, _, context) => context.Items[nameof(ApplicationUser)]))
            .ForMember(item => item.UpdatedAt, expression => expression.Ignore())
            .ForMember(item => item.UpdatedBy, expression => expression.Ignore())
            .ForMember(item => item.Ready, expression => expression.Ignore())
            .ForMember(item => item.Order, expression => expression.Ignore())
            .ForMember(item => item.PrintingDetails, expression => expression.Ignore())
            ;

        CreateMap<Item, ItemViewModel>();

        CreateMap<IPagedList<Item>, IPagedList<ItemViewModel>>()
            .ConvertUsing<PagedListConverter<Item, ItemViewModel>>();
    }
}