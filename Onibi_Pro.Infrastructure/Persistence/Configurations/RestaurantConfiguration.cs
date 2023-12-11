using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        ConfigureRestaurantTable(builder);
        ConfigureOrderIdTable(builder);
        ConfigureManagerTable(builder);
        ConfigureEmployeeTable(builder);

        builder.Metadata.FindNavigation(nameof(Restaurant.Managers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Restaurant.Employees))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureRestaurantTable(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => RestaurantId.Create(value));

        builder.OwnsOne(x => x.Address, ab =>
        {
            ab.Property(x => x.Street);
            ab.Property(x => x.PostalCode);
            ab.Property(x => x.City);
            ab.Property(x => x.Country);
        });
    }

    private static void ConfigureEmployeeTable(EntityTypeBuilder<Restaurant> builder)
    {
        builder.OwnsMany(x => x.Employees, eb =>
        {
            eb.ToTable("Employees");

            eb.WithOwner().HasForeignKey("RestaurantId");
            eb.HasKey("Id", "RestaurantId");

            eb.Property(x => x.Id)
                .HasColumnName("EmployeeId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => EmployeeId.Create(value));

            eb.Property(x => x.FirstName)
                .HasMaxLength(250);
            eb.Property(x => x.LastName)
                .HasMaxLength(250);
            eb.Property(x => x.Email)
                .HasMaxLength(250);
            eb.Property(x => x.City)
                .HasMaxLength(250);

            eb.OwnsMany(x => x.Positions, pb =>
            {
                pb.ToTable("EmployeePositions");

                pb.Property(x => x.Position);
            });

            eb.Navigation(x => x.Positions).Metadata.SetField("_positions");
            eb.Navigation(x => x.Positions).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }

    private static void ConfigureOrderIdTable(EntityTypeBuilder<Restaurant> builder)
    {
        builder.OwnsMany(x => x.OrderIds, ob =>
        {
            ob.ToTable("OrderIds");
            ob.HasKey("Id");
            ob.WithOwner().HasForeignKey("RestaurantId");

            ob.Property(x => x.Value)
                .HasColumnName("OrderId")
                .ValueGeneratedNever();
        });
    }

    private static void ConfigureManagerTable(EntityTypeBuilder<Restaurant> builder)
    {
        builder.OwnsMany(x => x.Managers, mb =>
        {
            mb.ToTable("Managers");

            mb.WithOwner().HasForeignKey("RestaurantId");
            mb.HasKey("Id", "RestaurantId");

            mb.Property(x => x.Id)
                .HasColumnName("ManagerId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => ManagerId.Create(value));

            mb.Property(x => x.FirstName)
                .HasMaxLength(250);
            mb.Property(x => x.LastName)
                .HasMaxLength(250);
            mb.Property(x => x.Email)
                .HasMaxLength(250);
        });
    }
}
