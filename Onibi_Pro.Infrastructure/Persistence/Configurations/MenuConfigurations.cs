using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Persistence.Configurations;
public class MenuConfigurations : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        ConfigureMenuTable(builder);
        ConfigureMenuItemsTable(builder);

        builder.Metadata.FindNavigation(nameof(Menu.MenuItems))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureMenuTable(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => MenuId.Create(value));

        builder.Property(x => x.Name)
            .HasMaxLength(100);
    }

    private static void ConfigureMenuItemsTable(EntityTypeBuilder<Menu> builder)
    {
        builder.OwnsMany(x => x.MenuItems, mib =>
        {
            mib.ToTable("MenuItems");

            mib.WithOwner().HasForeignKey("MenuId");
            mib.HasKey("Id", "MenuId");
            mib.HasIndex("MenuId");

            mib.Property(x => x.Id)
                .HasColumnName("MenuItemId")
                .ValueGeneratedNever()
                .HasConversion(
                id => id.Value,
                value => MenuItemId.Create(value));

            mib.Property(x => x.Name).HasMaxLength(100);
            mib.Property(x => x.Price);

            mib.OwnsMany(x => x.Ingredients, a =>
            {
                a.ToTable("Ingredients");

                a.Property<int>("Id").ValueGeneratedOnAdd();
                a.HasKey("Id");

                a.Property(x => x.Name).HasMaxLength(100);
                a.Property(x => x.Unit).HasConversion(
                    v => v.ToString(),
                    v => (UnitType)Enum.Parse(typeof(UnitType), v));
                a.Property(x => x.Quantity);
            });
        });
    }
}
