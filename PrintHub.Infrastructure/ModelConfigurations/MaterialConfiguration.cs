using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class MaterialConfiguration : IdentityModelConfigurationBase<Material>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Material> builder)
    {
        builder.Property(material => material.Name).IsRequired();
        builder.Property(material => material.Price).IsRequired();
        builder.Property(material => material.Technology).IsRequired();
        builder.Property(material => material.Description).IsRequired();

        builder.HasMany(material => material.AvailableColors)
            .WithMany(color => color.Materials);
    }
}