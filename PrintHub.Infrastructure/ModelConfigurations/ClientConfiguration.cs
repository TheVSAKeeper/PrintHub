using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class ClientConfiguration : IdentityModelConfigurationBase<Client>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Client> builder)
    {
        builder.Property(c => c.FirstName).IsRequired();
        builder.Property(c => c.LastName).IsRequired();
        builder.Property(c => c.Address).IsRequired();
        builder.Property(c => c.Phone).IsRequired();
        builder.Property(c => c.Email).IsRequired();
    }
}