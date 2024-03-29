IF EXISTS (SELECT * FROM sys.databases WHERE name = 'Onibi_Pro')
BEGIN
    PRINT 'Database already exists. Skipping the script.';
    set noexec on
END
ELSE
BEGIN
    PRINT 'Database does not exist. Executing the script...';
	set noexec off
END
GO

USE [master]
GO
/****** Object:  Database [Onibi_Pro]    Script Date: 01.02.2024 19:04:59 ******/
CREATE DATABASE [Onibi_Pro]
 CONTAINMENT = NONE
GO
ALTER DATABASE [Onibi_Pro] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Onibi_Pro].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Onibi_Pro] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Onibi_Pro] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Onibi_Pro] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Onibi_Pro] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Onibi_Pro] SET ARITHABORT OFF 
GO
ALTER DATABASE [Onibi_Pro] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Onibi_Pro] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Onibi_Pro] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Onibi_Pro] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Onibi_Pro] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Onibi_Pro] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Onibi_Pro] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Onibi_Pro] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Onibi_Pro] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Onibi_Pro] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Onibi_Pro] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Onibi_Pro] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Onibi_Pro] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Onibi_Pro] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Onibi_Pro] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Onibi_Pro] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Onibi_Pro] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Onibi_Pro] SET RECOVERY FULL 
GO
ALTER DATABASE [Onibi_Pro] SET  MULTI_USER 
GO
ALTER DATABASE [Onibi_Pro] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Onibi_Pro] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Onibi_Pro] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Onibi_Pro] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Onibi_Pro] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Onibi_Pro', N'ON'
GO
ALTER DATABASE [Onibi_Pro] SET QUERY_STORE = OFF
GO
USE [Onibi_Pro]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Couriers]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Couriers](
	[CourierId] [uniqueidentifier] NOT NULL,
	[RegionalManagerId] [uniqueidentifier] NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Couriers] PRIMARY KEY CLUSTERED 
(
	[CourierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeePositions]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeePositions](
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[EmployeeRestaurantId] [uniqueidentifier] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_EmployeePositions] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[EmployeeRestaurantId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[RestaurantId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[City] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeesSchedules]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeesSchedules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[ScheduleId] [uniqueidentifier] NOT NULL,
	[ScheduleRestaurantId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EmployeesSchedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredients]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Unit] [nvarchar](max) NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[MenuItemId] [uniqueidentifier] NOT NULL,
	[MenuItemMenuId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Ingredients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Managers]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Managers](
	[ManagerId] [uniqueidentifier] NOT NULL,
	[RestaurantId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Managers] PRIMARY KEY CLUSTERED 
(
	[ManagerId] ASC,
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuItems]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItems](
	[MenuItemId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY CLUSTERED 
(
	[MenuItemId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[MenuItemId] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderTime] [datetime2](7) NOT NULL,
	[IsCancelled] [bit] NOT NULL,
	[CancelledTime] [datetime2](7) NULL,
	[RestaurantId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Packages]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packages](
	[PackageId] [uniqueidentifier] NOT NULL,
	[DestinationRestaurant] [uniqueidentifier] NOT NULL,
	[Manager] [uniqueidentifier] NOT NULL,
	[RegionalManager] [uniqueidentifier] NOT NULL,
	[SourceRestaurant] [uniqueidentifier] NULL,
	[Courier] [uniqueidentifier] NULL,
	[Origin_Street] [nvarchar](max) NULL,
	[Origin_City] [nvarchar](max) NULL,
	[Origin_PostalCode] [nvarchar](max) NULL,
	[Origin_Country] [nvarchar](max) NULL,
	[Destination_Street] [nvarchar](max) NOT NULL,
	[Destination_City] [nvarchar](max) NOT NULL,
	[Destination_PostalCode] [nvarchar](max) NOT NULL,
	[Destination_Country] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[Message] [nvarchar](250) NOT NULL,
	[IsUrgent] [bit] NOT NULL,
	[Ingredients] [nvarchar](max) NOT NULL,
	[Until] [datetime2](7) NOT NULL,
	[AvailableTransitions] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegionalManagerRestaurantIds]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegionalManagerRestaurantIds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantId] [uniqueidentifier] NOT NULL,
	[RegionalManagerId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RegionalManagerRestaurantIds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegionalManagers]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegionalManagers](
	[RegionalManagerId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RegionalManagers] PRIMARY KEY CLUSTERED 
(
	[RegionalManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurants]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurants](
	[Id] [uniqueidentifier] NOT NULL,
	[Address_Street] [nvarchar](max) NOT NULL,
	[Address_City] [nvarchar](max) NOT NULL,
	[Address_PostalCode] [nvarchar](max) NOT NULL,
	[Address_Country] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Restaurants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[ScheduleId] [uniqueidentifier] NOT NULL,
	[RestaurantId] [uniqueidentifier] NOT NULL,
	[Priority] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](125) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC,
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPasswords]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPasswords](
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserPasswords] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 01.02.2024 19:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[UserType] [nvarchar](max) NOT NULL,
	[IsEmailConfirmed] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231210003309_InitialCreate', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231210163704_InitialCreate2', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231210163934_InitialCreate3', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231210171921_InitialCreate4', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231211004210_InitialCreate5', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231211151232_InitialCreate8', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231211215812_InitialCreate11', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231216211506_Migration_20231216221444', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231217224555_Migration_20231217234520', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231217235114_Migration_20231218005030', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231218002826_Migration_20231218012751', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231218231522_Migration_20231219001437', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231224193332_Migration_20231224203314', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231224205209_Migration_20231224215145', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231224224608_Migration_20231224234531', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231226201336_Migration_20231226211317', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231228192036_Migration_20231228201923', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231228192355_Migration_20231228202334', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231228224046_Migration_20231228234007', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231229204130_Migration_20231229214109', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231229204940_Migration_20231229214922', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231229234923_Migration_20231230004845', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231230011522_Migration_20231230021413', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231230155615_Migration_20231230165541', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231230160021_Migration_20231230165957', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240101143307_Migration_20240101153207', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240101202132_Migration_20240101212114', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240101233053_Migration_20240102003008', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240114153149_Migration_20240114163056', N'8.0.1')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240121004219_Migration_20240121014101', N'8.0.1')
INSERT [dbo].[Couriers] ([CourierId], [RegionalManagerId], [Phone], [UserId]) VALUES (N'f9311ec2-4b61-42fb-b2e3-025c6bd42744', N'be0313ee-13d5-4fbe-b900-367280b74501', N'+1 555-1234-5678', N'54b90822-f98e-4c4d-a3f4-674b67e8b2b9')
SET IDENTITY_INSERT [dbo].[EmployeePositions] ON 

INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c7f25580-5c9c-413f-b81b-8f8183f3416d', N'f2008daa-bf82-4014-83f0-8e1b011e7801', 2, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c7f25580-5c9c-413f-b81b-8f8183f3416d', N'f2008daa-bf82-4014-83f0-8e1b011e7801', 3, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'e17025ef-0803-4a4a-89ff-993ac315cdad', N'f2008daa-bf82-4014-83f0-8e1b011e7801', 1, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c9bc0e42-b30e-4ebc-b572-e5673603c8a2', N'f2008daa-bf82-4014-83f0-8e1b011e7801', 4, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c9bc0e42-b30e-4ebc-b572-e5673603c8a2', N'f2008daa-bf82-4014-83f0-8e1b011e7801', 5, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c9bc0e42-b30e-4ebc-b572-e5673603c8a2', N'f2008daa-bf82-4014-83f0-8e1b011e7801', 6, 3)
SET IDENTITY_INSERT [dbo].[EmployeePositions] OFF
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'c7f25580-5c9c-413f-b81b-8f8183f3416d', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'Ava', N'Wilson', N'ava.wilson@email.com', N'Houston')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'e17025ef-0803-4a4a-89ff-993ac315cdad', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'Ethan', N'Carter', N'ethan.miller@email.com', N'Dallas')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'c9bc0e42-b30e-4ebc-b572-e5673603c8a2', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'Mason', N'Taylor', N'mason.taylor@email.com', N'Austin')
SET IDENTITY_INSERT [dbo].[EmployeesSchedules] ON 

INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (1, N'c9bc0e42-b30e-4ebc-b572-e5673603c8a2', N'3f367ea9-be58-4cf7-bd89-eeebeb17cd5c', N'f2008daa-bf82-4014-83f0-8e1b011e7801')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (2, N'e17025ef-0803-4a4a-89ff-993ac315cdad', N'58b7c0b6-0054-4fd3-ace9-b732f9ce7369', N'f2008daa-bf82-4014-83f0-8e1b011e7801')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (3, N'c7f25580-5c9c-413f-b81b-8f8183f3416d', N'121074d3-e05e-43bf-8ba2-167301eaf25b', N'f2008daa-bf82-4014-83f0-8e1b011e7801')
SET IDENTITY_INSERT [dbo].[EmployeesSchedules] OFF
SET IDENTITY_INSERT [dbo].[Ingredients] ON 

INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (1, N'Beef Patty', N'Grams', CAST(150.00 AS Decimal(18, 2)), N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (2, N'Cheddar Cheese', N'Pieces', CAST(2.00 AS Decimal(18, 2)), N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (3, N'Pickles', N'Pieces', CAST(3.00 AS Decimal(18, 2)), N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (4, N'Onion Rings', N'Pieces', CAST(4.00 AS Decimal(18, 2)), N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (5, N'Burger Sauce', N'Milliliters', CAST(30.00 AS Decimal(18, 2)), N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (6, N'Bun', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (7, N'Chicken', N'Grams', CAST(100.00 AS Decimal(18, 2)), N'fff61d61-d8e5-435a-8c18-e577e2be554a', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (8, N'Breading', N'Grams', CAST(50.00 AS Decimal(18, 2)), N'fff61d61-d8e5-435a-8c18-e577e2be554a', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (9, N'BBQ Sauce', N'Milliliters', CAST(50.00 AS Decimal(18, 2)), N'fff61d61-d8e5-435a-8c18-e577e2be554a', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (10, N'Potato Fries', N'Grams', CAST(200.00 AS Decimal(18, 2)), N'e24dd78e-c82f-4873-9561-7b6c6425c790', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (11, N'Bacon Bits', N'Grams', CAST(50.00 AS Decimal(18, 2)), N'e24dd78e-c82f-4873-9561-7b6c6425c790', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (12, N'Cheddar Cheese Sauce', N'Milliliters', CAST(60.00 AS Decimal(18, 2)), N'e24dd78e-c82f-4873-9561-7b6c6425c790', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (13, N'Green Onions', N'Grams', CAST(10.00 AS Decimal(18, 2)), N'e24dd78e-c82f-4873-9561-7b6c6425c790', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (14, N'Sausage', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'7fae6425-e369-42ab-9012-b49f2c120b3f', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (15, N'Hot Dog Bun', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'7fae6425-e369-42ab-9012-b49f2c120b3f', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (16, N'Mustard', N'Milliliters', CAST(20.00 AS Decimal(18, 2)), N'7fae6425-e369-42ab-9012-b49f2c120b3f', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (17, N'Ketchup', N'Milliliters', CAST(20.00 AS Decimal(18, 2)), N'7fae6425-e369-42ab-9012-b49f2c120b3f', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (18, N'Relish', N'Milliliters', CAST(15.00 AS Decimal(18, 2)), N'7fae6425-e369-42ab-9012-b49f2c120b3f', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (19, N'Beef Mince', N'Grams', CAST(120.00 AS Decimal(18, 2)), N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (20, N'Taco Shell', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (21, N'Jalapenos', N'Pieces', CAST(5.00 AS Decimal(18, 2)), N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (22, N'Shredded Lettuce', N'Grams', CAST(30.00 AS Decimal(18, 2)), N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (23, N'Grated Cheese', N'Grams', CAST(40.00 AS Decimal(18, 2)), N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (24, N'Sour Cream', N'Milliliters', CAST(20.00 AS Decimal(18, 2)), N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (25, N'Meatballs', N'Pieces', CAST(4.00 AS Decimal(18, 2)), N'7d19a32d-3576-474b-8e90-06e899f278c5', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (26, N'Marinara Sauce', N'Milliliters', CAST(60.00 AS Decimal(18, 2)), N'7d19a32d-3576-474b-8e90-06e899f278c5', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (27, N'Provolone Cheese', N'Pieces', CAST(2.00 AS Decimal(18, 2)), N'7d19a32d-3576-474b-8e90-06e899f278c5', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (28, N'Sub Roll', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'7d19a32d-3576-474b-8e90-06e899f278c5', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (29, N'Parmesan Cheese', N'Grams', CAST(15.00 AS Decimal(18, 2)), N'7d19a32d-3576-474b-8e90-06e899f278c5', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (30, N'Pulled Pork', N'Grams', CAST(150.00 AS Decimal(18, 2)), N'e4abdf4c-3665-4a94-a2c4-9c583d8499e2', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (31, N'BBQ Sauce', N'Milliliters', CAST(50.00 AS Decimal(18, 2)), N'e4abdf4c-3665-4a94-a2c4-9c583d8499e2', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (32, N'Coleslaw', N'Grams', CAST(70.00 AS Decimal(18, 2)), N'e4abdf4c-3665-4a94-a2c4-9c583d8499e2', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (33, N'Sandwich Bun', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'e4abdf4c-3665-4a94-a2c4-9c583d8499e2', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (34, N'Chicken Wings', N'Pieces', CAST(5.00 AS Decimal(18, 2)), N'acead927-3618-4bd1-b718-13f2aa43e3c0', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (35, N'Flour', N'Grams', CAST(50.00 AS Decimal(18, 2)), N'acead927-3618-4bd1-b718-13f2aa43e3c0', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (36, N'Hot Sauce', N'Milliliters', CAST(30.00 AS Decimal(18, 2)), N'acead927-3618-4bd1-b718-13f2aa43e3c0', N'aefdd3d2-6680-412e-92e5-641568aa399d')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (37, N'Butter', N'Grams', CAST(20.00 AS Decimal(18, 2)), N'acead927-3618-4bd1-b718-13f2aa43e3c0', N'aefdd3d2-6680-412e-92e5-641568aa399d')
SET IDENTITY_INSERT [dbo].[Ingredients] OFF
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'3381e97c-c885-4809-8807-b26523d15d68', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'881ce67c-7695-472a-97f8-4c74f9129c92')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'4013ea9d-7353-4138-a48d-c9264f782546', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'653766d5-8848-49ce-acd8-5512fbd14ee9')
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'7d19a32d-3576-474b-8e90-06e899f278c5', N'aefdd3d2-6680-412e-92e5-641568aa399d', N'Giant Meatball Sub', CAST(6.49 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'acead927-3618-4bd1-b718-13f2aa43e3c0', N'aefdd3d2-6680-412e-92e5-641568aa399d', N'Fried Chicken Wings', CAST(4.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'e24dd78e-c82f-4873-9561-7b6c6425c790', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30', N'Loaded Fries', CAST(4.49 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'e4abdf4c-3665-4a94-a2c4-9c583d8499e2', N'aefdd3d2-6680-412e-92e5-641568aa399d', N'BBQ Pulled Pork Sandwich', CAST(5.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'9239c228-0207-43b8-806b-9c778e5f3084', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30', N'Cheeseburger Deluxe', CAST(5.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'7fae6425-e369-42ab-9012-b49f2c120b3f', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30', N'Hot Dog Classic', CAST(2.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'e25a6cb3-dc77-4718-ba83-bebf995c840c', N'aefdd3d2-6680-412e-92e5-641568aa399d', N'Spicy Beef Taco', CAST(2.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'fff61d61-d8e5-435a-8c18-e577e2be554a', N'966378f5-ec9e-47f5-9d3e-72cf606e8b30', N'Chicken Nuggets', CAST(3.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[Menus] ([Id], [Name]) VALUES (N'aefdd3d2-6680-412e-92e5-641568aa399d', N'Street Food Craze')
INSERT [dbo].[Menus] ([Id], [Name]) VALUES (N'966378f5-ec9e-47f5-9d3e-72cf606e8b30', N'Fast Food Fiesta')
SET IDENTITY_INSERT [dbo].[OrderItem] ON 

INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'fff61d61-d8e5-435a-8c18-e577e2be554a', 1, N'51b1dd47-379a-4ddb-a3c4-9e966b47405c', 4)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'e24dd78e-c82f-4873-9561-7b6c6425c790', 2, N'51b1dd47-379a-4ddb-a3c4-9e966b47405c', 5)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'7d19a32d-3576-474b-8e90-06e899f278c5', 7, N'4968444d-47d3-4ab7-9656-b8d5afb0964a', 6)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'7fae6425-e369-42ab-9012-b49f2c120b3f', 2, N'4968444d-47d3-4ab7-9656-b8d5afb0964a', 7)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'e25a6cb3-dc77-4718-ba83-bebf995c840c', 1, N'd4648c12-bddc-4bd5-bed8-d26270a4e35e', 1)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'7d19a32d-3576-474b-8e90-06e899f278c5', 3, N'd4648c12-bddc-4bd5-bed8-d26270a4e35e', 2)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'e4abdf4c-3665-4a94-a2c4-9c583d8499e2', 1, N'd4648c12-bddc-4bd5-bed8-d26270a4e35e', 3)
SET IDENTITY_INSERT [dbo].[OrderItem] OFF
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'51b1dd47-379a-4ddb-a3c4-9e966b47405c', CAST(N'2024-02-01T17:48:44.9943886' AS DateTime2), 1, CAST(N'2024-02-01T17:49:14.9945000' AS DateTime2), N'f2008daa-bf82-4014-83f0-8e1b011e7801')
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'4968444d-47d3-4ab7-9656-b8d5afb0964a', CAST(N'2024-02-01T17:48:59.7059384' AS DateTime2), 0, NULL, N'f2008daa-bf82-4014-83f0-8e1b011e7801')
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'd4648c12-bddc-4bd5-bed8-d26270a4e35e', CAST(N'2024-02-01T17:46:50.3514929' AS DateTime2), 0, NULL, N'f2008daa-bf82-4014-83f0-8e1b011e7801')
SET IDENTITY_INSERT [dbo].[RegionalManagerRestaurantIds] ON 

INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (1, N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'be0313ee-13d5-4fbe-b900-367280b74501')
SET IDENTITY_INSERT [dbo].[RegionalManagerRestaurantIds] OFF
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'be0313ee-13d5-4fbe-b900-367280b74501', N'404dc246-2213-4ae8-a183-dda7376653d4')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'33 Lone Star Avenue', N'Houston', N'77001', N'Houston')
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'121074d3-e05e-43bf-8ba2-167301eaf25b', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'Critical', N'Drive-through', CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 8) AS DateTime2), CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 20) AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'58b7c0b6-0054-4fd3-ace9-b732f9ce7369', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'Essential', N'Cooking', CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 5) AS DateTime2), CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 14) AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'3f367ea9-be58-4cf7-bd89-eeebeb17cd5c', N'f2008daa-bf82-4014-83f0-8e1b011e7801', N'Standard', N'Cleaning', CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1) AS DateTime2), CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 6) AS DateTime2))
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'881ce67c-7695-472a-97f8-4c74f9129c92', N'kGZc4whD4PO5N9wZ9wBDh1aWj7dbX3Yg71KguW2ENm9NXKR68fQzh7GNxX3CsA7XI1s7E7X92zeK0OhAM5VK9Q==.U56guBl/CGcNDy8bAaSupHrynj6rZX5nJWjeck7HrVJSqCPXnvq1U8iGV+s4CZenSxmfTHmqzTqUksx8CDnxKQ==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'653766d5-8848-49ce-acd8-5512fbd14ee9', N'oasXv2m3+s/G5PO5/LiJYgVUkNV+PxlW81Sa4uaA/R0XIlg+XqShn6E0Xe6IW0LTVit42qfgEebSk+z8bRDUGg==.8fTlEQ3yrxAVj9wgCz6fXPK3z3qIu+0uR1Bpy/o2r26JqvQJ2nJ7QOmlIzH5XRaMW/XYRR/AKrpPwSLMzDE8OQ==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'54b90822-f98e-4c4d-a3f4-674b67e8b2b9', N'jZLo7QyA4rFtzZ9nOx+9ny4uZCGRSlKINOly0EVfH+jhm50CqBC67WmHvPz4OZ8kmBO9bNofaLubtbWUCBOLxA==.wXU+C/WfH2TJMEaFM9PwBDGdQ/+l29BRwPEBrxyiwq6Hioke8wf7C4p8MlY/wXRxkJcdxd7O7aQimrOPx5/9Xg==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'f59cf698-6f65-4902-8593-87e790931cbf', N'3f2r2U+/pGcqw2fXwVkG1erdIltqgC1JJwizv9/113Y6Q9DIn10nOzWCC7IvzAOPbVOFjMERnLa6FSRaLMMePg==.U97cBJbhSC/OEYpDNdF/Ik5IIhwIVlLPl9Ao7Olrf2IoVhfrT3gK+06Rnp1blDBDyljZp1gxdmanUoLplXnkkQ==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'404dc246-2213-4ae8-a183-dda7376653d4', N'Jm+D3Ibh+dGfVlBoDnXKRcaa9LHRz4o9IqGOoqr90qZJpL2y5thf8sgUn8KSJYr0ElUYc10KG1Lr6HW2EkL7jw==.FNxrX6cfqtHzMa6KPghgPy3/REP6LZ9YMJgA9GEseRqqeaIG9LdArvpOqqNSug9yIq6WeT5H/jm6wvFm4STXrA==')
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'881ce67c-7695-472a-97f8-4c74f9129c92', N'Benjamin', N'Mitchell', N'benjaminManager@mcDowell.com', N'Manager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'653766d5-8848-49ce-acd8-5512fbd14ee9', N'Emily', N'Turner', N'emilyManager@mcDowell.com', N'Manager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'54b90822-f98e-4c4d-a3f4-674b67e8b2b9', N'Chloe', N'Anderson', N'chloeCourier@mcDowell.com', N'Courier', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'f59cf698-6f65-4902-8593-87e790931cbf', N'John', N'Johnson', N'globalManager@mcDowel.com', N'GlobalManager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'404dc246-2213-4ae8-a183-dda7376653d4', N'Oliver', N'Walker', N'regionalManager@mcDowell.com', N'RegionalManager', 1)
/****** Object:  Index [IX_Couriers_RegionalManagerId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Couriers_RegionalManagerId] ON [dbo].[Couriers]
(
	[RegionalManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Couriers_UserId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Couriers_UserId] ON [dbo].[Couriers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Employees_RestaurantId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Employees_RestaurantId] ON [dbo].[Employees]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmployeesSchedules_ScheduleId_ScheduleRestaurantId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_EmployeesSchedules_ScheduleId_ScheduleRestaurantId] ON [dbo].[EmployeesSchedules]
(
	[ScheduleId] ASC,
	[ScheduleRestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ingredients_MenuItemId_MenuItemMenuId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Ingredients_MenuItemId_MenuItemMenuId] ON [dbo].[Ingredients]
(
	[MenuItemId] ASC,
	[MenuItemMenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Managers_RestaurantId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Managers_RestaurantId] ON [dbo].[Managers]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Managers_UserId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Managers_UserId] ON [dbo].[Managers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MenuItems_MenuId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_MenuItems_MenuId] ON [dbo].[MenuItems]
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_OrderTime_IsCancelled]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_OrderTime_IsCancelled] ON [dbo].[Orders]
(
	[OrderTime] DESC,
	[IsCancelled] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_RestaurantId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_RestaurantId] ON [dbo].[Orders]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Packages_DestinationRestaurant]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Packages_DestinationRestaurant] ON [dbo].[Packages]
(
	[DestinationRestaurant] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Packages_RegionalManager]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Packages_RegionalManager] ON [dbo].[Packages]
(
	[RegionalManager] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RegionalManagerRestaurantIds_RegionalManagerId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_RegionalManagerRestaurantIds_RegionalManagerId] ON [dbo].[RegionalManagerRestaurantIds]
(
	[RegionalManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RegionalManagers_UserId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_RegionalManagers_UserId] ON [dbo].[RegionalManagers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Schedules_RestaurantId]    Script Date: 01.02.2024 19:04:59 ******/
CREATE NONCLUSTERED INDEX [IX_Schedules_RestaurantId] ON [dbo].[Schedules]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Couriers] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [UserId]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT (N'') FOR [City]
GO
ALTER TABLE [dbo].[Managers] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [UserId]
GO
ALTER TABLE [dbo].[MenuItems] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [RestaurantId]
GO
ALTER TABLE [dbo].[Packages] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Until]
GO
ALTER TABLE [dbo].[Packages] ADD  DEFAULT (N'') FOR [AvailableTransitions]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsEmailConfirmed]
GO
ALTER TABLE [dbo].[Couriers]  WITH CHECK ADD  CONSTRAINT [FK_Couriers_RegionalManagers_RegionalManagerId] FOREIGN KEY([RegionalManagerId])
REFERENCES [dbo].[RegionalManagers] ([RegionalManagerId])
GO
ALTER TABLE [dbo].[Couriers] CHECK CONSTRAINT [FK_Couriers_RegionalManagers_RegionalManagerId]
GO
ALTER TABLE [dbo].[Couriers]  WITH CHECK ADD  CONSTRAINT [FK_Couriers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Couriers] CHECK CONSTRAINT [FK_Couriers_Users_UserId]
GO
ALTER TABLE [dbo].[EmployeePositions]  WITH CHECK ADD  CONSTRAINT [FK_EmployeePositions_Employees_EmployeeId_EmployeeRestaurantId] FOREIGN KEY([EmployeeId], [EmployeeRestaurantId])
REFERENCES [dbo].[Employees] ([EmployeeId], [RestaurantId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeePositions] CHECK CONSTRAINT [FK_EmployeePositions_Employees_EmployeeId_EmployeeRestaurantId]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Restaurants_RestaurantId] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Restaurants_RestaurantId]
GO
ALTER TABLE [dbo].[EmployeesSchedules]  WITH CHECK ADD  CONSTRAINT [FK_EmployeesSchedules_Schedules_ScheduleId_ScheduleRestaurantId] FOREIGN KEY([ScheduleId], [ScheduleRestaurantId])
REFERENCES [dbo].[Schedules] ([ScheduleId], [RestaurantId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeesSchedules] CHECK CONSTRAINT [FK_EmployeesSchedules_Schedules_ScheduleId_ScheduleRestaurantId]
GO
ALTER TABLE [dbo].[Ingredients]  WITH CHECK ADD  CONSTRAINT [FK_Ingredients_MenuItems_MenuItemId_MenuItemMenuId] FOREIGN KEY([MenuItemId], [MenuItemMenuId])
REFERENCES [dbo].[MenuItems] ([MenuItemId], [MenuId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ingredients] CHECK CONSTRAINT [FK_Ingredients_MenuItems_MenuItemId_MenuItemMenuId]
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD  CONSTRAINT [FK_Managers_Restaurants_RestaurantId] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Managers] CHECK CONSTRAINT [FK_Managers_Restaurants_RestaurantId]
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD  CONSTRAINT [FK_Managers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Managers] CHECK CONSTRAINT [FK_Managers_Users_UserId]
GO
ALTER TABLE [dbo].[MenuItems]  WITH CHECK ADD  CONSTRAINT [FK_MenuItems_Menus_MenuId] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MenuItems] CHECK CONSTRAINT [FK_MenuItems_Menus_MenuId]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Orders_OrderId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Restaurants_RestaurantId] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Restaurants_RestaurantId]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_RegionalManagers_RegionalManager] FOREIGN KEY([RegionalManager])
REFERENCES [dbo].[RegionalManagers] ([RegionalManagerId])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_RegionalManagers_RegionalManager]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Restaurants_DestinationRestaurant] FOREIGN KEY([DestinationRestaurant])
REFERENCES [dbo].[Restaurants] ([Id])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Restaurants_DestinationRestaurant]
GO
ALTER TABLE [dbo].[RegionalManagerRestaurantIds]  WITH CHECK ADD  CONSTRAINT [FK_RegionalManagerRestaurantIds_RegionalManagers_RegionalManagerId] FOREIGN KEY([RegionalManagerId])
REFERENCES [dbo].[RegionalManagers] ([RegionalManagerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RegionalManagerRestaurantIds] CHECK CONSTRAINT [FK_RegionalManagerRestaurantIds_RegionalManagers_RegionalManagerId]
GO
ALTER TABLE [dbo].[RegionalManagers]  WITH CHECK ADD  CONSTRAINT [FK_RegionalManagers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RegionalManagers] CHECK CONSTRAINT [FK_RegionalManagers_Users_UserId]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Restaurants_RestaurantId] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Restaurants_RestaurantId]
GO
ALTER TABLE [dbo].[UserPasswords]  WITH CHECK ADD  CONSTRAINT [FK_UserPasswords_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserPasswords] CHECK CONSTRAINT [FK_UserPasswords_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [Onibi_Pro] SET  READ_WRITE 
GO

set noexec off
