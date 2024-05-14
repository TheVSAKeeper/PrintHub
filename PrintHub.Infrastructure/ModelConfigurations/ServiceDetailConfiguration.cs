using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class ServiceDetailConfiguration : IdentityModelConfigurationBase<ServiceDetail>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<ServiceDetail> builder)
    {
        builder.Property(serviceDetail => serviceDetail.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(serviceDetail => serviceDetail.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(serviceDetail => serviceDetail.Price)
            .IsRequired();

        builder.HasOne(serviceDetail => serviceDetail.AdditionalService)
            .WithMany(additionalService => additionalService.ServiceDetails)
            .HasForeignKey(serviceDetail => serviceDetail.AdditionalServiceId);
    }
}