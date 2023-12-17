using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData(
        new IdentityRole<Guid>
        {
            Id = Guid.NewGuid(),
            Name = "Manager",
            NormalizedName = "MANAGER"
        },
        new IdentityRole<Guid>
        {
            Id = Guid.NewGuid(),
            Name = "RegionalManager",
            NormalizedName = "REGIONALMANAGER"
        },
        new IdentityRole<Guid>
        {
            Id = Guid.NewGuid(),
            Name = "GlobalManager",
            NormalizedName = "GLOBALMANAGER"
        });
    }
}