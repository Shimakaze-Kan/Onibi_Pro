﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Onibi_Pro.Infrastructure.Persistence;

#nullable disable

namespace Onibi_Pro.Infrastructure.Migrations
{
    [DbContext(typeof(OnibiProDbContext))]
    partial class OnibiProDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime?>("CancelledTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("OrderTime", "IsCancelled")
                        .IsDescending();

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.PackageAggregate.Package", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("PackageId");

                    b.Property<Guid?>("Courier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DestinationRestaurant")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsUrgent")
                        .HasColumnType("bit");

                    b.Property<Guid>("Manager")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<Guid>("RegionalManager")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SourceRestaurant")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Until")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DestinationRestaurant");

                    b.HasIndex("RegionalManager");

                    b.ToTable("Packages", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.RegionalManagerAggregate.RegionalManager", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RegionalManagerId");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RegionalManagers", (string)null);
                });

            modelBuilder.Entity("Onibi_Pro.Domain.RestaurantAggregate.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Restaurants", (string)null);
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

                            b1.OwnsMany("Onibi_Pro.Domain.Common.ValueObjects.Ingredient", "Ingredients", b2 =>
                                {
                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<Guid>("MenuItemId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("MenuItemMenuId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("nvarchar(100)");

                                    b2.Property<decimal>("Quantity")
                                        .HasColumnType("decimal(18,2)");

                                    b2.Property<string>("Unit")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("Id");

                                    b2.HasIndex("MenuItemId", "MenuItemMenuId");

                                    b2.ToTable("Ingredients", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("MenuItemId", "MenuItemMenuId");
                                });

                            b1.Navigation("Ingredients");
                        });

                    b.Navigation("MenuItems");
                });

            modelBuilder.Entity("Onibi_Pro.Domain.OrderAggregate.Order", b =>
                {
                    b.HasOne("Onibi_Pro.Domain.RestaurantAggregate.Restaurant", null)
                        .WithMany()
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("Onibi_Pro.Domain.PackageAggregate.Package", b =>
                {
                    b.HasOne("Onibi_Pro.Domain.RestaurantAggregate.Restaurant", null)
                        .WithMany()
                        .HasForeignKey("DestinationRestaurant")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Onibi_Pro.Domain.RegionalManagerAggregate.RegionalManager", null)
                        .WithMany()
                        .HasForeignKey("RegionalManager")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.OwnsOne("Onibi_Pro.Domain.Common.ValueObjects.Address", "Destination", b1 =>
                        {
                            b1.Property<Guid>("PackageId")
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

                            b1.HasKey("PackageId");

                            b1.ToTable("Packages");

                            b1.WithOwner()
                                .HasForeignKey("PackageId");
                        });

                    b.OwnsOne("Onibi_Pro.Domain.Common.ValueObjects.Address", "Origin", b1 =>
                        {
                            b1.Property<Guid>("PackageId")
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

                            b1.HasKey("PackageId");

                            b1.ToTable("Packages");

                            b1.WithOwner()
                                .HasForeignKey("PackageId");
                        });

                    b.Navigation("Destination")
                        .IsRequired();

                    b.Navigation("Origin");
                });

            modelBuilder.Entity("Onibi_Pro.Domain.RegionalManagerAggregate.RegionalManager", b =>
                {
                    b.HasOne("Onibi_Pro.Domain.UserAggregate.User", null)
                        .WithOne()
                        .HasForeignKey("Onibi_Pro.Domain.RegionalManagerAggregate.RegionalManager", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Onibi_Pro.Domain.RegionalManagerAggregate.Entities.Courier", "Couriers", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("CourierId");

                            b1.Property<Guid>("RegionalManagerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Id", "RegionalManagerId");

                            b1.HasIndex("RegionalManagerId");

                            b1.ToTable("Couriers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RegionalManagerId");
                        });

                    b.OwnsMany("Onibi_Pro.Domain.RestaurantAggregate.ValueObjects.RestaurantId", "Restaurants", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("RegionalManagerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("RestaurantId");

                            b1.HasKey("Id");

                            b1.HasIndex("RegionalManagerId");

                            b1.ToTable("RegionalManagerRestaurantIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RegionalManagerId");
                        });

                    b.Navigation("Couriers");

                    b.Navigation("Restaurants");
                });

            modelBuilder.Entity("Onibi_Pro.Domain.RestaurantAggregate.Restaurant", b =>
                {
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

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id", "RestaurantId");

                            b1.HasIndex("RestaurantId");

                            b1.HasIndex("UserId")
                                .IsUnique();

                            b1.ToTable("Managers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");

                            b1.HasOne("Onibi_Pro.Domain.UserAggregate.User", null)
                                .WithOne()
                                .HasForeignKey("Onibi_Pro.Domain.RestaurantAggregate.Restaurant.Managers#Onibi_Pro.Domain.RestaurantAggregate.Entities.Manager", "UserId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });

                    b.OwnsMany("Onibi_Pro.Domain.RestaurantAggregate.Entities.Schedule", "Schedules", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ScheduleId");

                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("EndDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Priority")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("StartDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasMaxLength(125)
                                .HasColumnType("nvarchar(125)");

                            b1.HasKey("Id", "RestaurantId");

                            b1.HasIndex("RestaurantId");

                            b1.ToTable("Schedules", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");

                            b1.OwnsMany("Onibi_Pro.Domain.RestaurantAggregate.ValueObjects.EmployeeId", "Employees", b2 =>
                                {
                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<Guid>("ScheduleId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("ScheduleRestaurantId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("Value")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("EmployeeId");

                                    b2.HasKey("Id");

                                    b2.HasIndex("ScheduleId", "ScheduleRestaurantId");

                                    b2.ToTable("EmployeesSchedules", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ScheduleId", "ScheduleRestaurantId");
                                });

                            b1.Navigation("Employees");
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

                    b.Navigation("Schedules");
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
