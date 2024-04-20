using AutoMapper;
using PrintHub.Domain;
using PrintHub.WPF.Endpoints.MaterialEndpoints;

namespace PrintHub.WPF.Mappers;

public class MaterialMapperConfiguration : Profile
{
    public MaterialMapperConfiguration()
    {
        CreateMap<Material, MaterialViewModel>();
    }
}