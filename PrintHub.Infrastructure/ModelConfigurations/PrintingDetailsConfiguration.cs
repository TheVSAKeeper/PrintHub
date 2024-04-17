using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class PrintingDetailsConfiguration : IdentityModelConfigurationBase<PrintingDetails>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<PrintingDetails> builder)
    {
        builder.Property(pd => pd.Technology).IsRequired();
    }
}