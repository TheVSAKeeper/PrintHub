using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class SampleConfiguration : AuditableModelConfigurationBase<Sample>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Sample> builder)
    {
        builder.Property(s => s.Description).IsRequired();
        builder.Property(s => s.PrintingTechnology).IsRequired();
        builder.Property(s => s.Approved).IsRequired();

        builder.HasOne(s => s.Order)
            .WithMany(o => o.Samples)
            .HasForeignKey(s => s.OrderId);

        builder.HasOne(s => s.Material)
            .WithMany()
            .HasForeignKey(s => s.MaterialId);
    }
}