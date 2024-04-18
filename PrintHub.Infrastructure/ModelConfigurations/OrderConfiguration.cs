using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class OrderConfiguration : AuditableModelConfigurationBase<Order>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Description).IsRequired();
        builder.Property(o => o.Status).IsRequired();

        builder.HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId);
    }
}