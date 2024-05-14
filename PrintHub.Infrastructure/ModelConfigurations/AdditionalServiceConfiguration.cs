using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class AdditionalServiceConfiguration : IdentityModelConfigurationBase<AdditionalService>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<AdditionalService> builder)
    {
        builder.Property(additionalService => additionalService.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(additionalService => additionalService.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Navigation(additionalService => additionalService.ServiceDetails).AutoInclude();

        builder.HasMany(additionalService => additionalService.ServiceDetails)
            .WithOne(serviceDetail => serviceDetail.AdditionalService)
            .HasForeignKey(serviceDetail => serviceDetail.AdditionalServiceId);
    }
}