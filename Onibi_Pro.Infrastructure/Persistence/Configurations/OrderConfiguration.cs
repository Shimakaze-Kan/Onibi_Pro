using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.MenuAggregate.ValueObjects;
using Onibi_Pro.Domain.OrderAggregate;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ConfigureOrderTable(builder);
        ConfigureOrderItemsTable(builder);

        builder.Metadata.FindNavigation(nameof(Order.OrderItems))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureOrderTable(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.OrderTime, x.IsCancelled }).IsDescending();

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => OrderId.Create(value));

        builder.Property(x => x.OrderTime);
        builder.Property(x => x.IsCancelled);
        builder.Property(x => x.CancelledTime);
    }

    private static void ConfigureOrderItemsTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(x => x.OrderItems, oib =>
        {
            oib.Property(x => x.Quantity);
            oib.WithOwner().HasForeignKey("OrderId");

            oib.Property(x => x.MenuItemId)
                .HasConversion(
                id => id.Value,
                value => MenuItemId.Create(value));
        });
    }
}
