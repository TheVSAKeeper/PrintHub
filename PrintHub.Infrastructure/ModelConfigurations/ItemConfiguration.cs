using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class ItemConfiguration : AuditableModelConfigurationBase<Item>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Item> builder)
    {
        builder.Property(item => item.Description).IsRequired();
        builder.Property(item => item.Ready).IsRequired();

        builder.HasOne(item => item.Order)
            .WithMany(order => order.Items)
            .HasForeignKey(item => item.OrderId)
            .IsRequired();

        builder.HasOne(item => item.PrintingDetails)
            .WithMany(printingDetails => printingDetails.Items)
            .HasForeignKey(item => item.PrintingDetailsId)
            .IsRequired();
    }
}