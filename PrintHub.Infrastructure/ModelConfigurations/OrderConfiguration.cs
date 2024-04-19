using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class OrderConfiguration : AuditableModelConfigurationBase<Order>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Order> builder)
    {
        builder.Property(order => order.Description).IsRequired();
        builder.Property(order => order.Status).IsRequired();

        builder.HasOne(order => order.Client)
            .WithMany(client => client.Orders)
            .HasForeignKey(order => order.ClientId);
    }
}