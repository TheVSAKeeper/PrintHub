using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class SampleConfiguration : AuditableModelConfigurationBase<Sample>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Sample> builder)
    {
        builder.Property(sample => sample.Description).IsRequired();
        builder.Property(sample => sample.Approved).IsRequired();

        builder.HasOne(sample => sample.Order)
            .WithMany(order => order.Samples)
            .HasForeignKey(sample => sample.OrderId);

        builder.HasOne(sample => sample.PrintingDetails)
            .WithMany(printingDetails => printingDetails.Samples)
            .HasForeignKey(sample => sample.PrintingDetailsId);
    }
}