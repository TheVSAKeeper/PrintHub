using AutoMapper;
using Calabonga.PagedListCore;
using PrintHub.Domain;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderMapperConfiguration : Profile
{
    public OrderMapperConfiguration()
    {
        CreateMap<OrderCreateViewModel, Order>()
            .ForMember(order => order.RequiredColors, expression => expression.Ignore())
            .ForMember(order => order.CreatedAt, expression => expression.Ignore())
            .ForMember(order => order.CreatedBy, expression => expression.MapFrom((_, _, _, context) => context.Items[nameof(ApplicationUser)]))
            .ForMember(order => order.UpdatedAt, expression => expression.Ignore())
            .ForMember(order => order.UpdatedBy, expression => expression.Ignore())
            .ForMember(order => order.Status, expression => expression.MapFrom(_ => OrderStatus.New))
            .ForMember(order => order.Client, expression => expression.Ignore())
            .ForMember(order => order.Samples, expression => expression.Ignore())
            .ForMember(order => order.Items, expression => expression.Ignore())
            ;

        CreateMap<Order, OrderViewModel>();

        CreateMap<Order, OrderUpdateViewModel>();

        CreateMap<OrderUpdateViewModel, Order>()
            .ForMember(order => order.CreatedAt, expression => expression.Ignore())
            .ForMember(order => order.CreatedBy, expression => expression.Ignore())
            .ForMember(order => order.UpdatedAt, expression => expression.Ignore())
            .ForMember(order => order.UpdatedBy, expression => expression.MapFrom((_, _, _, context) => context.Items[nameof(ApplicationUser)]))
            .ForMember(order => order.Status, expression => expression.Ignore())
            .ForMember(order => order.ClientId, expression => expression.Ignore())
            .ForMember(order => order.Client, expression => expression.Ignore())
            .ForMember(order => order.Samples, expression => expression.Ignore())
            .ForMember(order => order.Items, expression => expression.Ignore())
            ;

        CreateMap<IPagedList<Order>, IPagedList<OrderViewModel>>()
            .ConvertUsing<PagedListConverter<Order, OrderViewModel>>();
    }
}