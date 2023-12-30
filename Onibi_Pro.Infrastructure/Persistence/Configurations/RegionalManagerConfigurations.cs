using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;


namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class RegionalManagerConfigurations : IEntityTypeConfiguration<RegionalManager>
{
    public void Configure(EntityTypeBuilder<RegionalManager> builder)
    {
        ConfigureRegionalManagersTable(builder);
        ConfigureCouriersTable(builder);
        ConfigureRestaurantIdsTable(builder);

        builder.Metadata.FindNavigation(nameof(RegionalManager.Couriers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(RegionalManager.Restaurants))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureRegionalManagersTable(EntityTypeBuilder<RegionalManager> builder)
    {
        builder.ToTable("RegionalManagers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("RegionalManagerId")
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => RegionalManagerId.Create(value));

        builder.Property(x => x.UserId)
            .HasConversion(c => c.Value, value => UserId.Create(value));
        builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<RegionalManager>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.FirstName);
        builder.Property(x => x.LastName);
    }

    private static void ConfigureCouriersTable(EntityTypeBuilder<RegionalManager> builder)
    {
        builder.OwnsMany(x => x.Couriers, cb =>
        {
            cb.ToTable("Couriers");

            cb.WithOwner().HasForeignKey("RegionalManagerId");
            cb.HasKey("Id", "RegionalManagerId");

            cb.Property(x => x.Id)
                .HasColumnName("CourierId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => CourierId.Create(value));
        });
    }

    private static void ConfigureRestaurantIdsTable(EntityTypeBuilder<RegionalManager> builder)
    {
        builder.OwnsMany(x => x.Restaurants, mb =>
        {
            mb.ToTable("RegionalManagerRestaurantIds");
            mb.HasKey("Id");
            mb.WithOwner().HasForeignKey("RegionalManagerId");
            mb.Property(x => x.Value)
                .HasColumnName("RestaurantId")
                .ValueGeneratedNever();
        });
    }
}
