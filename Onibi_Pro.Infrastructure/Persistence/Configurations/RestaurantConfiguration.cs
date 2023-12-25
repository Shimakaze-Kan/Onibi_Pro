using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        ConfigureRestaurantTable(builder);
        ConfigureOrderIdTable(builder);
        ConfigureManagerTable(builder);
        ConfigureEmployeeTable(builder);
        ConfigureScheduleTable(builder);

        builder.Metadata.FindNavigation(nameof(Restaurant.Managers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Restaurant.Employees))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(Restaurant.OrderIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Restaurant.OrderIds))!
            .SetField("_orders");
        
        builder.Metadata.FindNavigation(nameof(Restaurant.Schedules))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Restaurant.Schedules))!
            .SetField("_schedules");
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

            mb.Property(x => x.UserId)
                .HasConversion(
                    id => id.Value,
                    guid => UserId.Create(guid))
                .IsRequired();

            mb.HasOne<User>()
                .WithOne()
                .HasForeignKey<Manager>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureScheduleTable(EntityTypeBuilder<Restaurant> builder)
    {
        builder.OwnsMany(x => x.Schedules, sb =>
        {
            sb.ToTable("Schedules");

            sb.WithOwner().HasForeignKey("RestaurantId");
            sb.HasKey("Id", "RestaurantId");

            sb.Property(x => x.Id)
                .HasColumnName("ScheduleId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => ScheduleId.Create(value));

            sb.Property(x => x.Title)
                .HasMaxLength(125);
            sb.Property(x => x.StartDate);
            sb.Property(x => x.EndDate);
            sb.Property(x => x.Priority)
                .HasConversion(
                    priority => priority.ToString(),
                    value => Enum.Parse<Priorities>(value));

            sb.OwnsMany(x => x.Employees, eb =>
            {
                eb.ToTable("EmployeesSchedules");

                eb.Property<int>("Id")
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();
                eb.HasKey("Id");

                eb.Property(x => x.Value)
                    .ValueGeneratedNever()
                    .HasColumnName("EmployeeId");
            });


            sb.Navigation(x => x.Employees).Metadata.SetField("_employeeIds");
            sb.Navigation(x => x.Employees).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
