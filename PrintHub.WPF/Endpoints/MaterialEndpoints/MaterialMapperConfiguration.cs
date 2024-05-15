using Calabonga.PagedListCore;
using PrintHub.WPF.Definitions.Mapping;
using PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.MaterialEndpoints;

public class MaterialMapperConfiguration : Profile
{
    public MaterialMapperConfiguration()
    {
        CreateMap<Material, MaterialViewModel>();

        CreateMap<IPagedList<Material>, IPagedList<MaterialViewModel>>()
            .ConvertUsing<PagedListConverter<Material, MaterialViewModel>>();
    }
}