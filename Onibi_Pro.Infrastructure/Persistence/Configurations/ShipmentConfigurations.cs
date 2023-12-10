using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public sealed class ShipmentConfigurations : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        ConfigureShipmentTable(builder);
        ConfigureCouriers(builder);
        ConfigurePackages(builder);

        builder.Metadata.FindNavigation(nameof(Shipment.Couriers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(Shipment.Packages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureShipmentTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipment");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => ShipmentId.Create(value));
    }

    private static void ConfigurePackages(EntityTypeBuilder<Shipment> builder)
    {
        builder.OwnsMany(x => x.Packages, pb =>
        {
            pb.ToTable("Packages");

            pb.WithOwner().HasForeignKey("ShipmentId");
            pb.HasKey("Id", "ShipmentId");

            pb.Property(x => x.Id)
                .HasColumnName("PackageId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => PackageId.Create(value));

            pb.OwnsOne(x => x.Origin, ob =>
            {
                ob.Property(x => x.Street);
                ob.Property(x => x.PostalCode);
                ob.Property(x => x.City);
                ob.Property(x => x.Country);
            });

            pb.OwnsOne(x => x.Destination, db =>
            {
                db.Property(x => x.Street);
                db.Property(x => x.PostalCode);
                db.Property(x => x.City);
                db.Property(x => x.Country);
            });

            pb.Property(x => x.Manager)
                .HasConversion(
                id => id.Value,
                value => ManagerId.Create(value));

            pb.Property(x => x.RestaurantSourceManager)
                .HasConversion(
                id => id.Value,
                value => ManagerId.Create(value));

            pb.Property(x => x.Courier)
                .HasConversion(
                id => id.Value,
                value => CourierId.Create(value));

            pb.Property(x => x.RegionalManager)
                .HasConversion(
                id => id.Value,
                value => RegionalManagerId.Create(value));

            pb.Property(x => x.Status);
            pb.Property(x => x.Message)
                .HasMaxLength(250);
            pb.Property(x => x.IsUrgent);

            pb.Property(x => x.Ingredients)
                .HasConversion(
                 i => JsonSerializer.Serialize(i, new JsonSerializerOptions()),
                 i => JsonSerializer.Deserialize<List<Ingredient>>(i, new JsonSerializerOptions()) ?? new());
        });
    }

    private static void ConfigureCouriers(EntityTypeBuilder<Shipment> builder)
    {
        builder.OwnsMany(x => x.Couriers, cb =>
        {
            cb.ToTable("Couriers");
            cb.WithOwner().HasForeignKey("ShipmentId");
            cb.HasKey("Id", "ShipmentId");

            cb.Property(x => x.Id)
                .HasColumnName("CourierId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => CourierId.Create(value));

            cb.Property(x => x.Email)
                .HasMaxLength(100);
            cb.Property(x => x.Phone)
                .HasMaxLength(15);
        });
    }
}
