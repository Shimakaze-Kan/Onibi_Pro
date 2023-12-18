using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Infrastructure.Authentication.DbModels;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public sealed class UserPasswordConfigurations : IEntityTypeConfiguration<UserPassword>
{
    public void Configure(EntityTypeBuilder<UserPassword> builder)
    {
        builder.ToTable("UserPasswords");

        builder.HasKey(up => up.UserId);

        builder.Property(up => up.UserId)
            .HasConversion(
                id => id.Value,
                guid => UserId.Create(guid))
            .IsRequired();

        builder.Property(up => up.Password).IsRequired();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<UserPassword>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
