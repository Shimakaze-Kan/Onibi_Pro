﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.Entities;
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
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureCouriersTable(EntityTypeBuilder<RegionalManager> builder)
    {
        builder.HasMany<Courier>(nameof(RegionalManager.Couriers))
            .WithOne()
            .HasForeignKey("RegionalManagerId");
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
