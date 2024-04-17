using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintHub.Domain.Base;

namespace PrintHub.Infrastructure.ModelConfigurations.Base;

public abstract class IdentityModelConfigurationBase<T> : ModelConfigurationBase<T> where T : Identity
{
    protected sealed override void AddBaseConfiguration(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        AddCustomConfiguration(builder);
    }

    protected abstract void AddCustomConfiguration(EntityTypeBuilder<T> builder);
}