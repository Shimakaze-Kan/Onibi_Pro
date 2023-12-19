﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Onibi_Pro.Infrastructure.Persistence;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    [DbContext(typeof(OnibiProDbContext))]
    [Migration("20231217235114_Migration_20231218005030")]
    partial class Migration_20231218005030
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Onibi_Pro.Domain.MenuAggregate.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Menus", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.RestaurantAggregate.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Restaurants", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.ShipmentAggregate.Shipment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Shipment", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Infrastructure.Authentication.DbModels.UserPassword", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserPasswords", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.MenuAggregate.Menu", b =>
                {
                    b.OwnsMany("Onibi_Pro.Domain.MenuAggregate.Entities.MenuItem", "MenuItems", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("MenuItemId");

                            b1.Property<Guid>("MenuId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Ingredients")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<decimal>("Price")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("Id", "MenuId");

                            b1.HasIndex("MenuId");

                            b1.ToTable("MenuItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MenuId");
                        });

                    b.Navigation("MenuItems");
                });

            modelBuilder.Entity("Onibi_Pro.Domain.OrderAggregate.Order", b =>
                {
                    b.OwnsMany("Onibi_Pro.Domain.OrderAggregate.ValueObjects.OrderItem", "OrderItems", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("MenuItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int");

                            b1.HasKey("OrderId", "Id");

                            b1.ToTable("OrderItem");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Onibi_Pro.Domain.RestaurantAggregate.Restaurant", b =>
                {
                    b.OwnsMany("Onibi_Pro.Domain.OrderAggregate.ValueObjects.OrderId", "OrderIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("OrderId");

                            b1.HasKey("Id");

                            b1.HasIndex("RestaurantId");

                            b1.ToTable("OrderIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.OwnsMany("Onibi_Pro.Domain.RestaurantAggregate.Entities.Employee", "Employees", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("EmployeeId");

                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.HasKey("Id", "RestaurantId");

                            b1.HasIndex("RestaurantId");

                            b1.ToTable("Employees", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");

                            b1.OwnsMany("Onibi_Pro.Domain.RestaurantAggregate.ValueObjects.EmployeePosition", "Positions", b2 =>
                                {
                                    b2.Property<Guid>("EmployeeId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("EmployeeRestaurantId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<int>("Position")
                                        .HasColumnType("int");

                                    b2.HasKey("EmployeeId", "EmployeeRestaurantId", "Id");

                                    b2.ToTable("EmployeePositions", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("EmployeeId", "EmployeeRestaurantId");
                                });

                            b1.Navigation("Positions");
                        });

                    b.OwnsMany("Onibi_Pro.Domain.RestaurantAggregate.Entities.Manager", "Managers", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ManagerId");

                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.HasKey("Id", "RestaurantId");

                            b1.HasIndex("RestaurantId");

                            b1.ToTable("Managers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.OwnsOne("Onibi_Pro.Domain.Common.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("RestaurantId");

                            b1.ToTable("Restaurants");

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Employees");

                    b.Navigation("Managers");

                    b.Navigation("OrderIds");
                });

            modelBuilder.Entity("Onibi_Pro.Domain.ShipmentAggregate.Shipment", b =>
                {
                    b.OwnsMany("Onibi_Pro.Domain.ShipmentAggregate.Entities.Courier", "Couriers", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("CourierId");

                            b1.Property<Guid>("ShipmentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasMaxLength(15)
                                .HasColumnType("nvarchar(15)");

                            b1.HasKey("Id", "ShipmentId");

                            b1.HasIndex("ShipmentId");

                            b1.ToTable("Couriers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ShipmentId");
                        });

                    b.OwnsMany("Onibi_Pro.Domain.ShipmentAggregate.Entities.Package", "Packages", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("PackageId");

                            b1.Property<Guid>("ShipmentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid?>("Courier")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Ingredients")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("IsUrgent")
                                .HasColumnType("bit");

                            b1.Property<Guid>("Manager")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Message")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)");

                            b1.Property<Guid>("RegionalManager")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid?>("RestaurantSourceManager")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Status")
                                .HasColumnType("int");

                            b1.HasKey("Id", "ShipmentId");

                            b1.HasIndex("ShipmentId");

                            b1.ToTable("Packages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ShipmentId");

                            b1.OwnsOne("Onibi_Pro.Domain.Common.ValueObjects.Address", "Destination", b2 =>
                                {
                                    b2.Property<Guid>("PackageId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("PackageShipmentId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("City")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("Country")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("PostalCode")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("Street")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("PackageId", "PackageShipmentId");

                                    b2.ToTable("Packages");

                                    b2.WithOwner()
                                        .HasForeignKey("PackageId", "PackageShipmentId");
                                });

                            b1.OwnsOne("Onibi_Pro.Domain.Common.ValueObjects.Address", "Origin", b2 =>
                                {
                                    b2.Property<Guid>("PackageId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("PackageShipmentId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("City")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("Country")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("PostalCode")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("Street")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("PackageId", "PackageShipmentId");

                                    b2.ToTable("Packages");

                                    b2.WithOwner()
                                        .HasForeignKey("PackageId", "PackageShipmentId");
                                });

                            b1.Navigation("Destination")
                                .IsRequired();

                            b1.Navigation("Origin");
                        });

                    b.Navigation("Couriers");

                    b.Navigation("Packages");
                });

            modelBuilder.Entity("Onibi_Pro.Infrastructure.Authentication.DbModels.UserPassword", b =>
                {
                    b.HasOne("Onibi_Pro.Domain.UserAggregate.User", null)
                        .WithOne()
                        .HasForeignKey("Onibi_Pro.Infrastructure.Authentication.DbModels.UserPassword", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}