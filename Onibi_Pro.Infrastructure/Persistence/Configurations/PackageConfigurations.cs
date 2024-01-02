using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class PackageConfigurations : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        ConfigurePackagesTable(builder);
    }

    private static void ConfigurePackagesTable(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("Packages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("PackageId")
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => PackageId.Create(value));

        builder.OwnsOne(x => x.Origin, ob =>
        {
            ob.Property(x => x.Street);
            ob.Property(x => x.PostalCode);
            ob.Property(x => x.City);
            ob.Property(x => x.Country);
        });

        builder.OwnsOne(x => x.Destination, db =>
        {
            db.Property(x => x.Street);
            db.Property(x => x.PostalCode);
            db.Property(x => x.City);
            db.Property(x => x.Country);
        });

        builder.Property(x => x.Manager)
            .HasConversion(
            id => id.Value,
            value => ManagerId.Create(value));

        builder.Property(x => x.SourceRestaurant)
            .HasConversion(
            id => id.Value,
            value => RestaurantId.Create(value));

        builder.Property(x => x.Courier)
            .HasConversion(
            id => id.Value,
            value => CourierId.Create(value));

        builder.Property(x => x.DestinationRestaurant)
            .HasConversion(
            id => id.Value,
            value => RestaurantId.Create(value));

        builder.HasOne<Restaurant>()
            .WithMany()
            .HasForeignKey(x => x.DestinationRestaurant)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.RegionalManager)
            .HasConversion(
            id => id.Value,
            value => RegionalManagerId.Create(value));

        builder.HasOne<RegionalManager>()
            .WithMany()
            .HasForeignKey(x => x.RegionalManager)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Status);
        builder.Property(x => x.Message)
            .HasMaxLength(250);
        builder.Property(x => x.IsUrgent);
        builder.Property(x => x.Until);

        builder.Property(x => x.Ingredients)
            .HasConversion(
             i => JsonSerializer.Serialize(i, new JsonSerializerOptions()),
             i => ConvertDtoToIngredientList(JsonSerializer.Deserialize<List<IngredientJson>>(i, new JsonSerializerOptions())));

        builder.Property(x => x.AvailableTransitions)
            .HasConversion(
             i => JsonSerializer.Serialize(i, new JsonSerializerOptions()),
             i => JsonSerializer.Deserialize<List<ShipmentStatus>>(i, new JsonSerializerOptions()) ?? new());
    }

    private static List<Ingredient> ConvertDtoToIngredientList(List<IngredientJson>? ingredientJsons)
    {
        if (ingredientJsons is null)
            return new();

        return ingredientJsons.ConvertAll(json => Ingredient.Create(json.Name, json.Unit, json.Quantity));
    }

    private class IngredientJson
    {
        public string Name { get; set; } = "";
        public UnitType Unit { get; set; }
        public decimal Quantity { get; set; }
    }
}
