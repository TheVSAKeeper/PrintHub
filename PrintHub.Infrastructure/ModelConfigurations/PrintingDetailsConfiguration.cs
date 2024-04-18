using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class PrintingDetailsConfiguration : IdentityModelConfigurationBase<PrintingDetails>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<PrintingDetails> builder)
    {
        builder.Property(printingDetails => printingDetails.Technology).IsRequired();

        builder.HasOne(printingDetails => printingDetails.Color)
            .WithMany(color => color.PrintingDetails)
            .HasForeignKey(printingDetails => printingDetails.ColorId)
            .IsRequired();

        builder.HasOne(printingDetails => printingDetails.Material)
            .WithMany(material => material.PrintingDetails)
            .HasForeignKey(printingDetails => printingDetails.MaterialId)
            .IsRequired();
    }
}