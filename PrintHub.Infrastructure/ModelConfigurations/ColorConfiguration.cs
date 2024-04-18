using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class ColorConfiguration : IdentityModelConfigurationBase<Color>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Color> builder)
    {
        builder.Property(color => color.Name).IsRequired();
        builder.Property(color => color.ColorCode).IsRequired();
    }
}