using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => UserId.Create(value));

        builder.Property(x => x.FirstName);
        builder.Property(x => x.LastName);
        builder.Property(x => x.Email);
        builder.Property(x => x.UserType)
            .HasConversion(
            type => type.ToString(),
            value => Enum.Parse<UserTypes>(value));
    }
}
