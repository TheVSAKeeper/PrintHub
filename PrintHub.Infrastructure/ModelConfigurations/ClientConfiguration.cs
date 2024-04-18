using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain;
using PrintHub.Infrastructure.ModelConfigurations.Base;

namespace PrintHub.Infrastructure.ModelConfigurations;

public class ClientConfiguration : IdentityModelConfigurationBase<Client>
{
    protected override void AddCustomConfiguration(EntityTypeBuilder<Client> builder)
    {
        builder.Property(client => client.FirstName).IsRequired();
        builder.Property(client => client.LastName).IsRequired();
        builder.Property(client => client.Address).IsRequired();
        builder.Property(client => client.Phone).IsRequired();
        builder.Property(client => client.Email).IsRequired();
    }
}