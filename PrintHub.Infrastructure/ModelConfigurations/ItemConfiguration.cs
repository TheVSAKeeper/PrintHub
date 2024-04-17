using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class ItemConfiguration : AuditableModelConfigurationBase<Item>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Item> builder)
    {
        builder.Property(i => i.Description).IsRequired();
        builder.Property(i => i.PrintingTechnology).IsRequired();
        builder.Property(i => i.Ready).IsRequired();

        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId);

        builder.HasOne(i => i.Material)
            .WithMany()
            .HasForeignKey(i => i.MaterialId);
    }
}