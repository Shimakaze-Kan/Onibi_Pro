using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.RegionalManagerAggregate.Entities;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class CourierConfigurations : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.ToTable("Couriers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("CourierId")
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => CourierId.Create(value));

        builder.Property(x => x.Id)
            .HasConversion(c => c.Value, value => CourierId.Create(value));

        builder.Property(x => x.UserId)
            .HasConversion(c => c.Value, value => UserId.Create(value));
        builder.Property(x => x.Phone).IsRequired();

        builder.HasOne<User>().WithMany().HasForeignKey(p => p.UserId);
    }
}
