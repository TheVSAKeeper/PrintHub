using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class MaterialConfiguration : IdentityModelConfigurationBase<Material>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Material> builder)
    {
        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Price).IsRequired();
        builder.Property(m => m.Description).IsRequired();

        builder.HasMany(m => m.AvailableColors)
            .WithOne(c => c.Material)
            .HasForeignKey(c => c.MaterialId);

        builder.HasOne(m => m.PrintingDetails)
            .WithOne()
            .HasForeignKey<PrintingDetails>(pd => pd.MaterialId);
    }
}