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
/****** Object:  Database [Onibi_Pro]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Couriers]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[EmployeePositions]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Employees]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[EmployeesSchedules]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Ingredients]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Managers]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[MenuItems]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Menus]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[OrderItem]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Orders]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Packages]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[RegionalManagerRestaurantIds]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[RegionalManagers]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Restaurants]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Schedules]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[UserPasswords]    Script Date: 29.01.2024 00:18:56 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 29.01.2024 00:18:56 ******/
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
INSERT [dbo].[Couriers] ([CourierId], [RegionalManagerId], [Phone], [UserId]) VALUES (N'ca856ca6-7608-4f69-b719-78a1470a3522', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', N'1234234', N'f0658c94-aab2-41c4-b3ac-9445f8c72da6')
INSERT [dbo].[Couriers] ([CourierId], [RegionalManagerId], [Phone], [UserId]) VALUES (N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', N'twoja stara', N'e7a4344b-6141-422d-9f61-5421958ed8b4')
SET IDENTITY_INSERT [dbo].[EmployeePositions] ON 

INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'6485d55a-6a8b-4044-8832-131972631446', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 122, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'6485d55a-6a8b-4044-8832-131972631446', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 123, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'6b223a57-e41a-4866-b500-152056ecc3bc', N'196ff048-cdc3-4c7e-a411-87feaa249ac5', 79, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'6b223a57-e41a-4866-b500-152056ecc3bc', N'196ff048-cdc3-4c7e-a411-87feaa249ac5', 80, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 90, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 91, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 92, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'05617c18-76c1-4b4a-a55d-2a3b1b074b50', N'd7163a81-3605-4036-9e56-0ad014c7b9fc', 17, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'05617c18-76c1-4b4a-a55d-2a3b1b074b50', N'd7163a81-3605-4036-9e56-0ad014c7b9fc', 18, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'275e7860-06e7-4583-8e3a-2e412d037398', N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', 11, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'275e7860-06e7-4583-8e3a-2e412d037398', N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', 12, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'957e6d31-b860-4646-8bd7-34c2cc1f56f9', N'56ed1931-58cd-4215-a207-c47612868ecc', 7, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'957e6d31-b860-4646-8bd7-34c2cc1f56f9', N'56ed1931-58cd-4215-a207-c47612868ecc', 8, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'aaac07dc-dfdd-401f-ab20-36e1b2fa6ab8', N'54de692c-287b-4944-910e-42a9fa8a475a', 63, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'aaac07dc-dfdd-401f-ab20-36e1b2fa6ab8', N'54de692c-287b-4944-910e-42a9fa8a475a', 64, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'30dbd386-f46c-416f-9a19-3926cdf4ecb7', N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', 53, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'30dbd386-f46c-416f-9a19-3926cdf4ecb7', N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', 54, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ee7e1c7e-3f7a-4730-a28c-3c5b7e2dd9f3', N'd7163a81-3605-4036-9e56-0ad014c7b9fc', 19, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ee7e1c7e-3f7a-4730-a28c-3c5b7e2dd9f3', N'd7163a81-3605-4036-9e56-0ad014c7b9fc', 20, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0f73e4d1-1030-4b4f-8bc8-3eabf743c991', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 120, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0f73e4d1-1030-4b4f-8bc8-3eabf743c991', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 121, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9713ea5b-0312-4603-9b11-3fdc67b94ffd', N'00e062e2-57d4-4b22-a6f4-009b785f6583', 3, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9713ea5b-0312-4603-9b11-3fdc67b94ffd', N'00e062e2-57d4-4b22-a6f4-009b785f6583', 4, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'56828368-4784-4bb3-a971-402a1e5b3a78', N'79f86506-305a-4827-b1a4-edf786cef5e6', 49, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'56828368-4784-4bb3-a971-402a1e5b3a78', N'79f86506-305a-4827-b1a4-edf786cef5e6', 50, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'27a92839-389a-4d3e-9e3a-41909bb79da6', N'dd42de9d-7fe7-4540-a869-22ae4a402223', 29, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'27a92839-389a-4d3e-9e3a-41909bb79da6', N'dd42de9d-7fe7-4540-a869-22ae4a402223', 30, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ccb2502c-5e03-483f-9c8e-41962d920fb8', N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', 33, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ccb2502c-5e03-483f-9c8e-41962d920fb8', N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', 34, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'838a017a-73a8-49da-8e70-49c4809d4f5c', N'8f11efa1-875b-489c-8a30-7e4414369f9a', 41, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'838a017a-73a8-49da-8e70-49c4809d4f5c', N'8f11efa1-875b-489c-8a30-7e4414369f9a', 42, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'3e2da2b5-7333-4bfe-829c-4d25fd9e386b', N'196ff048-cdc3-4c7e-a411-87feaa249ac5', 77, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'3e2da2b5-7333-4bfe-829c-4d25fd9e386b', N'196ff048-cdc3-4c7e-a411-87feaa249ac5', 78, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'a406dfa5-bdbc-42b8-ac97-509bb440bd12', N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', 83, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'a406dfa5-bdbc-42b8-ac97-509bb440bd12', N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', 84, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'27e1ea5e-bd9e-456a-a058-516c33af2c5b', N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', 81, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'27e1ea5e-bd9e-456a-a058-516c33af2c5b', N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', 82, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 124, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 125, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 126, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'66e35965-2956-4c51-9fd9-69aa90ea7fab', N'dd42de9d-7fe7-4540-a869-22ae4a402223', 31, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'66e35965-2956-4c51-9fd9-69aa90ea7fab', N'dd42de9d-7fe7-4540-a869-22ae4a402223', 32, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ca1cf7af-c7a1-4546-bc2d-6b1089990519', N'b073d450-d585-4ab0-a908-9edf1e8e001b', 47, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ca1cf7af-c7a1-4546-bc2d-6b1089990519', N'b073d450-d585-4ab0-a908-9edf1e8e001b', 48, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'f458bab8-8424-4516-afe0-7326a87eb9f4', N'8b95345e-269e-4ca2-8012-3163b7dc0237', 27, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'f458bab8-8424-4516-afe0-7326a87eb9f4', N'8b95345e-269e-4ca2-8012-3163b7dc0237', 28, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'b834714b-6e33-4759-a012-75f27bcfe602', N'79f86506-305a-4827-b1a4-edf786cef5e6', 51, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'b834714b-6e33-4759-a012-75f27bcfe602', N'79f86506-305a-4827-b1a4-edf786cef5e6', 52, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ea7581f1-2d3e-4a2e-a1a6-75fb32a43e0e', N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', 35, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'ea7581f1-2d3e-4a2e-a1a6-75fb32a43e0e', N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', 36, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'3a37ddfd-69b1-4031-8a5a-77513895eb10', N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', 57, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'3a37ddfd-69b1-4031-8a5a-77513895eb10', N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', 58, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'e960f199-e9d4-4758-9759-78d1c2a06d86', N'8f11efa1-875b-489c-8a30-7e4414369f9a', 43, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'e960f199-e9d4-4758-9759-78d1c2a06d86', N'8f11efa1-875b-489c-8a30-7e4414369f9a', 44, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'41e53840-7868-4faf-b48e-7c1f44d7441e', N'f5e01790-d313-4b65-b42d-a01a1a21be1f', 69, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'41e53840-7868-4faf-b48e-7c1f44d7441e', N'f5e01790-d313-4b65-b42d-a01a1a21be1f', 70, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'6abda4fd-5e23-4d65-b768-7f5c59724cde', N'c3f95301-9d15-4c27-967f-6f791d304653', 21, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'6abda4fd-5e23-4d65-b768-7f5c59724cde', N'c3f95301-9d15-4c27-967f-6f791d304653', 22, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'8fa4817f-4c1f-4802-8d93-891fa1fb4798', N'520dc622-9921-4337-b0b9-d7bd271a76a0', 67, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'8fa4817f-4c1f-4802-8d93-891fa1fb4798', N'520dc622-9921-4337-b0b9-d7bd271a76a0', 68, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c2016b8f-33d8-44e4-97ba-95b12f687ab7', N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', 59, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'c2016b8f-33d8-44e4-97ba-95b12f687ab7', N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', 60, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9949c203-195e-4887-b5d3-977749d044a5', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 127, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9949c203-195e-4887-b5d3-977749d044a5', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 128, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'445449f4-1949-4aa8-86eb-9daccc7d497a', N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', 39, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'445449f4-1949-4aa8-86eb-9daccc7d497a', N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', 40, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'16fa439f-06cb-41c6-8139-a84f472173f7', N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', 37, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'16fa439f-06cb-41c6-8139-a84f472173f7', N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', 38, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'805671e8-3b03-44e0-b70e-acacfaabd2ab', N'a89332ec-feda-44bc-bc09-223a600f6398', 73, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'805671e8-3b03-44e0-b70e-acacfaabd2ab', N'a89332ec-feda-44bc-bc09-223a600f6398', 74, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'f165be5b-f4d6-4afb-a9b0-b22a39a85a4f', N'c3f95301-9d15-4c27-967f-6f791d304653', 23, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'f165be5b-f4d6-4afb-a9b0-b22a39a85a4f', N'c3f95301-9d15-4c27-967f-6f791d304653', 24, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'cd0300f2-239e-4207-807c-b7200d3ed6f8', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 96, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9071f1ef-c98d-4878-8759-b797e90217e9', N'f5e01790-d313-4b65-b42d-a01a1a21be1f', 71, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9071f1ef-c98d-4878-8759-b797e90217e9', N'f5e01790-d313-4b65-b42d-a01a1a21be1f', 72, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'a3e48de6-c435-475f-b0f2-c0e606fb13f9', N'8b95345e-269e-4ca2-8012-3163b7dc0237', 25, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'a3e48de6-c435-475f-b0f2-c0e606fb13f9', N'8b95345e-269e-4ca2-8012-3163b7dc0237', 26, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'e6d45b50-3a20-41d0-878d-c2daaa435cfa', N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', 55, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'e6d45b50-3a20-41d0-878d-c2daaa435cfa', N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', 56, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'69cd806f-ad6f-4b41-ada0-c3d02af1a487', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 85, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'69cd806f-ad6f-4b41-ada0-c3d02af1a487', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 86, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'95b0d6e7-8cbf-4f85-9bd9-d05ff07c4134', N'54de692c-287b-4944-910e-42a9fa8a475a', 61, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'95b0d6e7-8cbf-4f85-9bd9-d05ff07c4134', N'54de692c-287b-4944-910e-42a9fa8a475a', 62, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0511535b-99a1-405c-8e33-d0ffe4680a0c', N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', 9, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'0511535b-99a1-405c-8e33-d0ffe4680a0c', N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', 10, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'88cb8af1-f32a-4503-825b-d77bba836825', N'00e062e2-57d4-4b22-a6f4-009b785f6583', 1, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'88cb8af1-f32a-4503-825b-d77bba836825', N'00e062e2-57d4-4b22-a6f4-009b785f6583', 2, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9ac89f81-0d96-43d6-93a2-d8613b754126', N'a89332ec-feda-44bc-bc09-223a600f6398', 75, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'9ac89f81-0d96-43d6-93a2-d8613b754126', N'a89332ec-feda-44bc-bc09-223a600f6398', 76, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'8436cb30-936c-4c4b-854d-d8d29436667e', N'56ed1931-58cd-4215-a207-c47612868ecc', 5, 0)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'8436cb30-936c-4c4b-854d-d8d29436667e', N'56ed1931-58cd-4215-a207-c47612868ecc', 6, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'266b3153-dac3-48bc-8996-d9f8b530ba51', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 95, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'57e9ef86-66b6-4e3d-8e23-e67e8e0beadb', N'520dc622-9921-4337-b0b9-d7bd271a76a0', 65, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'57e9ef86-66b6-4e3d-8e23-e67e8e0beadb', N'520dc622-9921-4337-b0b9-d7bd271a76a0', 66, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'948a3c7b-b1a0-4b6f-a1a9-ec152e438068', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 87, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'948a3c7b-b1a0-4b6f-a1a9-ec152e438068', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 88, 3)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'948a3c7b-b1a0-4b6f-a1a9-ec152e438068', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 89, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'12c7fea3-ee9b-47fc-9409-f1dc3f82a41a', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 106, 1)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'12c7fea3-ee9b-47fc-9409-f1dc3f82a41a', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', 107, 2)
GO
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'bee597fc-bb65-4f18-8b34-fc687c6d5860', N'b073d450-d585-4ab0-a908-9edf1e8e001b', 45, 2)
INSERT [dbo].[EmployeePositions] ([EmployeeId], [EmployeeRestaurantId], [Id], [Position]) VALUES (N'bee597fc-bb65-4f18-8b34-fc687c6d5860', N'b073d450-d585-4ab0-a908-9edf1e8e001b', 46, 3)
SET IDENTITY_INSERT [dbo].[EmployeePositions] OFF
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'6485d55a-6a8b-4044-8832-131972631446', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Jane', N'Smith', N'jane.smith@example.com', N'Chicago')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'6b223a57-e41a-4866-b500-152056ecc3bc', N'196ff048-cdc3-4c7e-a411-87feaa249ac5', N'Jane', N'Smith', N'jane.smith@example.com', N'New York')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Alice', N'Wonderchef', N'alice.wonderf@example.com', N'Foodsville')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'05617c18-76c1-4b4a-a55d-2a3b1b074b50', N'd7163a81-3605-4036-9e56-0ad014c7b9fc', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'275e7860-06e7-4583-8e3a-2e412d037398', N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'957e6d31-b860-4646-8bd7-34c2cc1f56f9', N'56ed1931-58cd-4215-a207-c47612868ecc', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'aaac07dc-dfdd-401f-ab20-36e1b2fa6ab8', N'54de692c-287b-4944-910e-42a9fa8a475a', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'30dbd386-f46c-416f-9a19-3926cdf4ecb7', N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'ee7e1c7e-3f7a-4730-a28c-3c5b7e2dd9f3', N'd7163a81-3605-4036-9e56-0ad014c7b9fc', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'0f73e4d1-1030-4b4f-8bc8-3eabf743c991', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'John', N'Doe', N'john.doe@example.com', N'kkkk')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'9713ea5b-0312-4603-9b11-3fdc67b94ffd', N'00e062e2-57d4-4b22-a6f4-009b785f6583', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'56828368-4784-4bb3-a971-402a1e5b3a78', N'79f86506-305a-4827-b1a4-edf786cef5e6', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'27a92839-389a-4d3e-9e3a-41909bb79da6', N'dd42de9d-7fe7-4540-a869-22ae4a402223', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'ccb2502c-5e03-483f-9c8e-41962d920fb8', N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'838a017a-73a8-49da-8e70-49c4809d4f5c', N'8f11efa1-875b-489c-8a30-7e4414369f9a', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'3e2da2b5-7333-4bfe-829c-4d25fd9e386b', N'196ff048-cdc3-4c7e-a411-87feaa249ac5', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'a406dfa5-bdbc-42b8-ac97-509bb440bd12', N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'27e1ea5e-bd9e-456a-a058-516c33af2c5b', N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Jkm', N'korwin', N'hhhh@ppp.com', N'paróweczkowo')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'66e35965-2956-4c51-9fd9-69aa90ea7fab', N'dd42de9d-7fe7-4540-a869-22ae4a402223', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'ca1cf7af-c7a1-4546-bc2d-6b1089990519', N'b073d450-d585-4ab0-a908-9edf1e8e001b', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'f458bab8-8424-4516-afe0-7326a87eb9f4', N'8b95345e-269e-4ca2-8012-3163b7dc0237', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'b834714b-6e33-4759-a012-75f27bcfe602', N'79f86506-305a-4827-b1a4-edf786cef5e6', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'ea7581f1-2d3e-4a2e-a1a6-75fb32a43e0e', N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'3a37ddfd-69b1-4031-8a5a-77513895eb10', N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'e960f199-e9d4-4758-9759-78d1c2a06d86', N'8f11efa1-875b-489c-8a30-7e4414369f9a', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'41e53840-7868-4faf-b48e-7c1f44d7441e', N'f5e01790-d313-4b65-b42d-a01a1a21be1f', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'6abda4fd-5e23-4d65-b768-7f5c59724cde', N'c3f95301-9d15-4c27-967f-6f791d304653', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'8fa4817f-4c1f-4802-8d93-891fa1fb4798', N'520dc622-9921-4337-b0b9-d7bd271a76a0', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'c2016b8f-33d8-44e4-97ba-95b12f687ab7', N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'9949c203-195e-4887-b5d3-977749d044a5', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'dupal', N'ddd', N'dupa@dupa.com', N'Lagunica')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'445449f4-1949-4aa8-86eb-9daccc7d497a', N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'16fa439f-06cb-41c6-8139-a84f472173f7', N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'805671e8-3b03-44e0-b70e-acacfaabd2ab', N'a89332ec-feda-44bc-bc09-223a600f6398', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'f165be5b-f4d6-4afb-a9b0-b22a39a85a4f', N'c3f95301-9d15-4c27-967f-6f791d304653', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'cd0300f2-239e-4207-807c-b7200d3ed6f8', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'ggg', N'ggg', N'hg@oiui.gf', N'gggg')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'9071f1ef-c98d-4878-8759-b797e90217e9', N'f5e01790-d313-4b65-b42d-a01a1a21be1f', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'a3e48de6-c435-475f-b0f2-c0e606fb13f9', N'8b95345e-269e-4ca2-8012-3163b7dc0237', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'e6d45b50-3a20-41d0-878d-c2daaa435cfa', N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'69cd806f-ad6f-4b41-ada0-c3d02af1a487', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Alice', N'Wonderchef', N'alice.wonder@example.com', N'Foodville')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'95b0d6e7-8cbf-4f85-9bd9-d05ff07c4134', N'54de692c-287b-4944-910e-42a9fa8a475a', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'0511535b-99a1-405c-8e33-d0ffe4680a0c', N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'88cb8af1-f32a-4503-825b-d77bba836825', N'00e062e2-57d4-4b22-a6f4-009b785f6583', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'9ac89f81-0d96-43d6-93a2-d8613b754126', N'a89332ec-feda-44bc-bc09-223a600f6398', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'8436cb30-936c-4c4b-854d-d8d29436667e', N'56ed1931-58cd-4215-a207-c47612868ecc', N'John', N'Doe', N'john.doe@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'266b3153-dac3-48bc-8996-d9f8b530ba51', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'gdf', N'dgf', N'gdfg.dgf@g.gg', N'dgf')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'57e9ef86-66b6-4e3d-8e23-e67e8e0beadb', N'520dc622-9921-4337-b0b9-d7bd271a76a0', N'Jane', N'Smith', N'jane.smith@example.com', N'')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'948a3c7b-b1a0-4b6f-a1a9-ec152e438068', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Alice', N'Johnson', N'alice.j@example.com', N'San Francisco')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'12c7fea3-ee9b-47fc-9409-f1dc3f82a41a', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Donald', N'Trump Junior', N'realtrump@example.com', N'Foodsville')
INSERT [dbo].[Employees] ([EmployeeId], [RestaurantId], [FirstName], [LastName], [Email], [City]) VALUES (N'bee597fc-bb65-4f18-8b34-fc687c6d5860', N'b073d450-d585-4ab0-a908-9edf1e8e001b', N'Jane', N'Smith', N'jane.smith@example.com', N'')
SET IDENTITY_INSERT [dbo].[EmployeesSchedules] ON 

INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (2, N'0f73e4d1-1030-4b4f-8bc8-3eabf743c991', N'c41f23c1-31d0-4460-ab4c-1ac56c9967b8', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (3, N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'c41f23c1-31d0-4460-ab4c-1ac56c9967b8', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (17, N'6485d55a-6a8b-4044-8832-131972631446', N'b48d1430-2df5-4146-840e-949dd855da0a', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (26, N'6485d55a-6a8b-4044-8832-131972631446', N'ca88f9ef-1966-43f8-92a8-3cad1723a7a1', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (37, N'0f73e4d1-1030-4b4f-8bc8-3eabf743c991', N'c9653cb0-e2bd-4367-a2e0-b9570c35a6ac', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (43, N'0f73e4d1-1030-4b4f-8bc8-3eabf743c991', N'9d2f1a3a-53bc-4a1c-8313-b505abd63263', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (50, N'6485d55a-6a8b-4044-8832-131972631446', N'1898632a-eaa0-46e5-a289-26442b2183f1', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (51, N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'1898632a-eaa0-46e5-a289-26442b2183f1', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (52, N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'1898632a-eaa0-46e5-a289-26442b2183f1', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (54, N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'b045a695-0821-45e7-99fb-e6d5410fcdfb', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (55, N'fcb5c1c2-2d58-4393-a20a-618cf76552e3', N'b045a695-0821-45e7-99fb-e6d5410fcdfb', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[EmployeesSchedules] ([Id], [EmployeeId], [ScheduleId], [ScheduleRestaurantId]) VALUES (56, N'0cbc0f1b-2f6c-4255-a534-1f9e03bdabb9', N'b381deaf-1b31-4f3a-aa65-b46fcd1e7c15', N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
SET IDENTITY_INSERT [dbo].[EmployeesSchedules] OFF
SET IDENTITY_INSERT [dbo].[Ingredients] ON 

INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (19, N'Grilled Chicken', N'Grams', CAST(200.00 AS Decimal(18, 2)), N'038c4a1f-d524-4461-8fb3-1183ebb627dc', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (20, N'Mixed Greens', N'Grams', CAST(150.00 AS Decimal(18, 2)), N'038c4a1f-d524-4461-8fb3-1183ebb627dc', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (21, N'Cherry Tomatoes', N'Pieces', CAST(5.00 AS Decimal(18, 2)), N'038c4a1f-d524-4461-8fb3-1183ebb627dc', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (22, N'Avocado', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'038c4a1f-d524-4461-8fb3-1183ebb627dc', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (23, N'Balsamic Vinaigrette', N'Milliliters', CAST(40.00 AS Decimal(18, 2)), N'038c4a1f-d524-4461-8fb3-1183ebb627dc', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (24, N'Shrimp', N'Grams', CAST(150.00 AS Decimal(18, 2)), N'ef22833a-c67a-47e8-8a26-d455a5a2818b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (25, N'Scallops', N'Grams', CAST(100.00 AS Decimal(18, 2)), N'ef22833a-c67a-47e8-8a26-d455a5a2818b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (26, N'Linguine Pasta', N'Grams', CAST(200.00 AS Decimal(18, 2)), N'ef22833a-c67a-47e8-8a26-d455a5a2818b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (27, N'Tomato Sauce', N'Milliliters', CAST(100.00 AS Decimal(18, 2)), N'ef22833a-c67a-47e8-8a26-d455a5a2818b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (28, N'Parmesan Cheese', N'Grams', CAST(30.00 AS Decimal(18, 2)), N'ef22833a-c67a-47e8-8a26-d455a5a2818b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (29, N'Banana', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'61256c12-1ded-4365-a5f6-fedee536b37b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (30, N'Strawberries', N'Grams', CAST(100.00 AS Decimal(18, 2)), N'61256c12-1ded-4365-a5f6-fedee536b37b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (31, N'Mango', N'Pieces', CAST(1.00 AS Decimal(18, 2)), N'61256c12-1ded-4365-a5f6-fedee536b37b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (32, N'Orange Juice', N'Milliliters', CAST(150.00 AS Decimal(18, 2)), N'61256c12-1ded-4365-a5f6-fedee536b37b', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (33, N'Pizza Dough', N'Grams', CAST(200.00 AS Decimal(18, 2)), N'e0708ec6-ee40-4042-b94c-5a7a1745d40e', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (34, N'Tomato Sauce', N'Milliliters', CAST(80.00 AS Decimal(18, 2)), N'e0708ec6-ee40-4042-b94c-5a7a1745d40e', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (35, N'Fresh Mozzarella', N'Grams', CAST(150.00 AS Decimal(18, 2)), N'e0708ec6-ee40-4042-b94c-5a7a1745d40e', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (36, N'Basil', N'Grams', CAST(10.00 AS Decimal(18, 2)), N'e0708ec6-ee40-4042-b94c-5a7a1745d40e', N'e5138d89-0d89-473c-978e-a1bd74923487')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (37, N'test ingredient', N'Milliliters', CAST(23.00 AS Decimal(18, 2)), N'3b8d205d-5e0f-4aff-aea0-14346be06685', N'81deca71-e577-44e1-aa63-c8403065d03c')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (38, N'potato', N'Grams', CAST(200.00 AS Decimal(18, 2)), N'388b537b-69cf-4354-9a41-b1f74ec34853', N'81deca71-e577-44e1-aa63-c8403065d03c')
INSERT [dbo].[Ingredients] ([Id], [Name], [Unit], [Quantity], [MenuItemId], [MenuItemMenuId]) VALUES (39, N'oil', N'Milliliters', CAST(20.00 AS Decimal(18, 2)), N'388b537b-69cf-4354-9a41-b1f74ec34853', N'81deca71-e577-44e1-aa63-c8403065d03c')
SET IDENTITY_INSERT [dbo].[Ingredients] OFF
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'6a389f6d-cbbf-4c6c-a567-c8614e6ff398', N'd8e4e33e-1b62-4ccb-9eb0-6efe7e0f0161', N'bd966720-7a2e-44bb-9698-2b574d175713')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'05b28a71-5a72-4191-9b74-d117416b4f7a', N'd8e4e33e-1b62-4ccb-9eb0-6efe7e0f0161', N'd025a95e-6b84-40c8-ad63-4f0f59a250d2')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'b65a70dd-0530-46cf-9ea2-ed2f178f91f2', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'e7a4344b-6141-422d-9f61-5421958ed8b4')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'7ab0c9fd-c8e5-4b49-a23e-7f58ee65ebb6', N'00e062e2-57d4-4b22-a6f4-009b785f6583', N'c2a63572-66b9-41bb-a8a7-68c72ee14724')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'b74d4c2b-2774-41a0-bf0c-45e1c9a0fd4a', N'd8e4e33e-1b62-4ccb-9eb0-6efe7e0f0161', N'1b9ec2e8-b776-4bc1-8faa-7ab1ceebe531')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'6150a4cd-654e-49b2-82cb-c7e66b8cc8ce', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'f59cf698-6f65-4902-8593-87e790931cbf')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'ac25e3dc-bbbc-43c4-87a5-fc6806b42d6e', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'5450da1b-4622-4f0e-b7d1-a6f0c3b82bf4')
INSERT [dbo].[Managers] ([ManagerId], [RestaurantId], [UserId]) VALUES (N'1291cd42-b1d1-4b80-afdd-b59a93425cc8', N'd8e4e33e-1b62-4ccb-9eb0-6efe7e0f0161', N'1eda9289-413c-4c79-b764-c62552b0b5ea')
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'038c4a1f-d524-4461-8fb3-1183ebb627dc', N'e5138d89-0d89-473c-978e-a1bd74923487', N'Grilled Chicken Salad', CAST(8.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'3b8d205d-5e0f-4aff-aea0-14346be06685', N'81deca71-e577-44e1-aa63-c8403065d03c', N'test item', CAST(0.05 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'e0708ec6-ee40-4042-b94c-5a7a1745d40e', N'e5138d89-0d89-473c-978e-a1bd74923487', N'Classic Margherita Pizza', CAST(10.99 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'388b537b-69cf-4354-9a41-b1f74ec34853', N'81deca71-e577-44e1-aa63-c8403065d03c', N'french fries', CAST(0.01 AS Decimal(18, 2)), 0)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'ef22833a-c67a-47e8-8a26-d455a5a2818b', N'e5138d89-0d89-473c-978e-a1bd74923487', N'Seafood Pasta', CAST(12.99 AS Decimal(18, 2)), 1)
INSERT [dbo].[MenuItems] ([MenuItemId], [MenuId], [Name], [Price], [IsDeleted]) VALUES (N'61256c12-1ded-4365-a5f6-fedee536b37b', N'e5138d89-0d89-473c-978e-a1bd74923487', N'Fruit Smoothie', CAST(4.49 AS Decimal(18, 2)), 0)
INSERT [dbo].[Menus] ([Id], [Name]) VALUES (N'e5138d89-0d89-473c-978e-a1bd74923487', N'Deluxe Menu')
INSERT [dbo].[Menus] ([Id], [Name]) VALUES (N'81deca71-e577-44e1-aa63-c8403065d03c', N'test menu')
SET IDENTITY_INSERT [dbo].[OrderItem] ON 

INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'61256c12-1ded-4365-a5f6-fedee536b37b', 1435453, N'061605c5-4e06-453e-a9df-1a7bb4e7ee4a', 33)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'61256c12-1ded-4365-a5f6-fedee536b37b', 5, N'ccffa2fa-d8ae-4c9c-85cd-3e82951b3313', 32)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'ef22833a-c67a-47e8-8a26-d455a5a2818b', 3, N'6f0c65fa-ed6d-467f-8386-558f212a64b2', 29)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'ef22833a-c67a-47e8-8a26-d455a5a2818b', 5, N'd77034e5-93ca-4265-adbe-b73a47b04697', 30)
INSERT [dbo].[OrderItem] ([MenuItemId], [Quantity], [OrderId], [Id]) VALUES (N'61256c12-1ded-4365-a5f6-fedee536b37b', 1, N'd77034e5-93ca-4265-adbe-b73a47b04697', 31)
SET IDENTITY_INSERT [dbo].[OrderItem] OFF
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'061605c5-4e06-453e-a9df-1a7bb4e7ee4a', CAST(N'2023-12-31T15:32:16.2058558' AS DateTime2), 1, CAST(N'2023-12-31T15:32:28.1170017' AS DateTime2), N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'ccffa2fa-d8ae-4c9c-85cd-3e82951b3313', CAST(N'2023-12-29T21:13:38.4445202' AS DateTime2), 1, CAST(N'2023-12-29T21:13:49.4262891' AS DateTime2), N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'6f0c65fa-ed6d-467f-8386-558f212a64b2', CAST(N'2023-12-29T20:59:07.9473439' AS DateTime2), 1, CAST(N'2023-12-29T21:13:15.1943390' AS DateTime2), N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[Orders] ([Id], [OrderTime], [IsCancelled], [CancelledTime], [RestaurantId]) VALUES (N'd77034e5-93ca-4265-adbe-b73a47b04697', CAST(N'2023-12-29T21:13:30.3202606' AS DateTime2), 0, NULL, N'e2e115cf-4a20-40e4-add5-67cf34788a0a')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'1bd15f99-f9d2-4a26-a5f3-060df6d7579d', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, NULL, NULL, NULL, NULL, NULL, N'123 Main Street', N'Anytown', N'12345', N'USA', 6, N'reject test', 1, N'[{"Name":"Banana","Unit":2,"Quantity":1}]', CAST(N'2024-01-03T19:40:00.7780000' AS DateTime2), N'[]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'fa5ac3bd-886e-4d9c-9824-170094ac4bea', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, NULL, NULL, NULL, NULL, NULL, N'123 Main Street', N'Anytown', N'12345', N'USA', 0, N'deliver asap pls uwu', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":100},{"Name":"Orange Juice","Unit":1,"Quantity":25},{"Name":"Grilled Chicken","Unit":0,"Quantity":8}]', CAST(N'2024-01-03T18:46:50.3140000' AS DateTime2), N'[2,6,1]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'71e6f204-39a2-4e2f-809c-25afdab98b0d', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'rrrr', N'rrrr', N'rrr', N'rrr', N'123 Main Street', N'Anytown', N'12345', N'USA', 2, N'pls asap', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":1},{"Name":"Banana","Unit":2,"Quantity":16},{"Name":"Mixed Greens","Unit":0,"Quantity":1}]', CAST(N'2024-01-08T23:31:09.8560000' AS DateTime2), N'[4,6]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'49e08b37-7736-4b26-9ce7-2984032bd71e', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', N'845bf26a-97d5-4ef6-ab67-9734019494d5', N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'123 Main Street', N'Anytown', N'12345', N'USA', N'123 Main Street', N'Anytown', N'12345', N'USA', 1, N'', 1, N'[{"Name":"Balsamic Vinaigrette","Unit":1,"Quantity":1}]', CAST(N'2024-01-02T16:48:01.4100000' AS DateTime2), N'[3,6]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'41fe4ffd-ce0b-4dd4-b2fd-459cf8d0eb07', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', N'845bf26a-97d5-4ef6-ab67-9734019494d5', N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'123 Main Street', N'Anytown', N'12345', N'USA', N'123 Main Street', N'Anytown', N'12345', N'USA', 6, N'', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":1}]', CAST(N'2024-01-09T15:46:41.5150000' AS DateTime2), N'[]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'f76baeeb-173f-4d7a-9f21-4c540d7d0742', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'dupa', N'dupa', N'dupa', N'dupa', N'123 Main Street', N'Anytown', N'12345', N'USA', 5, N'asap pls', 1, N'[{"Name":"Banana","Unit":2,"Quantity":1}]', CAST(N'2024-01-09T01:30:11.4810000' AS DateTime2), N'[]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'28c67240-4d60-4ec1-ad4f-6470c5edc0cd', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'hghjhjjh', N'kmnhgkjhk', N'kjlkj', N'kjkj', N'123 Main Street', N'Anytown', N'12345', N'USA', 2, N'we really need this asap', 1, N'[{"Name":"Shrimp","Unit":0,"Quantity":13},{"Name":"Orange Juice","Unit":1,"Quantity":9},{"Name":"Banana","Unit":2,"Quantity":9},{"Name":"Grilled Chicken","Unit":0,"Quantity":1}]', CAST(N'2024-01-08T23:24:44.3250000' AS DateTime2), N'[4,6]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'7ebe44cf-c6ff-4e96-bb24-6c659bafab41', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'fsdffds', N'dsfdsf', N'dsfdsf', N'dsfdsf', N'123 Main Street', N'Anytown', N'12345', N'USA', 2, N'', 1, N'[{"Name":"Balsamic Vinaigrette","Unit":1,"Quantity":1}]', CAST(N'2024-01-02T16:46:16.2950000' AS DateTime2), N'[4,6]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'47b7e859-547a-41fd-b133-9b84a4ce035b', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'fdg', N'dgf', N'dgf', N'dgf', N'123 Main Street', N'Anytown', N'12345', N'USA', 2, N'', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":1}]', CAST(N'2024-01-08T23:46:28.0080000' AS DateTime2), N'[4,6]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'1a5e53d1-21e0-43cc-8551-a83ee84d4190', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, NULL, NULL, NULL, NULL, NULL, N'123 Main Street', N'Anytown', N'12345', N'USA', 0, N'', 1, N'[{"Name":"Balsamic Vinaigrette","Unit":1,"Quantity":1}]', CAST(N'2024-01-02T16:46:05.6270000' AS DateTime2), N'[2,6,1]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'ed13d3bb-e4c6-49b6-a33f-b3ac80da5165', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'uuuuuu', N'uuuuu', N'uuu', N'uuu', N'123 Main Street', N'Anytown', N'12345', N'USA', 5, N'test nr 2', 1, N'[{"Name":"Basil","Unit":0,"Quantity":4},{"Name":"Banana","Unit":2,"Quantity":9},{"Name":"Mango","Unit":2,"Quantity":1}]', CAST(N'2024-01-03T19:38:40.7280000' AS DateTime2), N'[]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'ed541840-a485-4eda-b693-c383b413e36f', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'twojej starej 69', N'piździsko', N'69-420', N'Galaktyka Kurwix', N'123 Main Street', N'Anytown', N'12345', N'USA', 2, N'please deliver asap', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":7},{"Name":"Linguine Pasta","Unit":0,"Quantity":10},{"Name":"Basil","Unit":0,"Quantity":4}]', CAST(N'2024-01-08T23:35:03.3860000' AS DateTime2), N'[4,6]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'a1892cad-a778-4a30-8d07-e4aac7a61893', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, NULL, NULL, NULL, NULL, NULL, N'123 Main Street', N'Anytown', N'12345', N'USA', 6, N'', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":1}]', CAST(N'2024-01-09T15:47:58.7410000' AS DateTime2), N'[]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'478ee207-ee9a-406a-a5dd-f84975726a9e', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', NULL, N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'ggggg', N'ddddd', N'dddd', N'jugosławia', N'123 Main Street', N'Anytown', N'12345', N'USA', 5, N'test nr 1', 1, N'[{"Name":"Avocado","Unit":2,"Quantity":2}]', CAST(N'2024-01-03T18:55:16.7770000' AS DateTime2), N'[]')
INSERT [dbo].[Packages] ([PackageId], [DestinationRestaurant], [Manager], [RegionalManager], [SourceRestaurant], [Courier], [Origin_Street], [Origin_City], [Origin_PostalCode], [Origin_Country], [Destination_Street], [Destination_City], [Destination_PostalCode], [Destination_Country], [Status], [Message], [IsUrgent], [Ingredients], [Until], [AvailableTransitions]) VALUES (N'16aa8a6b-ace9-4d80-b02e-fff1e191bd4e', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'672ad2ef-8c05-469b-8d61-b40724790bc7', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', N'845bf26a-97d5-4ef6-ab67-9734019494d5', N'f85f7997-9c32-43eb-9ece-a8039b83e036', N'123 Main Street', N'Anytown', N'12345', N'USA', N'123 Main Street', N'Anytown', N'12345', N'USA', 6, N'', 1, N'[{"Name":"Basil","Unit":0,"Quantity":1}]', CAST(N'2024-01-02T16:47:12.0900000' AS DateTime2), N'[]')
SET IDENTITY_INSERT [dbo].[RegionalManagerRestaurantIds] ON 

INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (1, N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (2, N'845bf26a-97d5-4ef6-ab67-9734019494d5', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (3, N'd8e4e33e-1b62-4ccb-9eb0-6efe7e0f0161', N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (4, N'00e062e2-57d4-4b22-a6f4-009b785f6583', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (5, N'd7163a81-3605-4036-9e56-0ad014c7b9fc', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (6, N'a2e4666f-95d1-4eb5-9079-8950bc1fd8ef', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (7, N'd49c8a4d-7be5-4fb1-bd6d-d617f1f0160c', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (8, N'83b1b437-87c0-4430-ba8f-d30d466cc478', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (9, N'5761104b-44f7-439c-9ce8-7edad54baef4', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (10, N'd46425a6-be79-48ea-8d52-3baf851b6dcf', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (11, N'46671117-1955-41ae-a254-bbded21a1fcf', N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (12, N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', N'0623bc31-8f01-4e69-b3df-e2a651c97479')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (13, N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', N'196bd483-0f8e-4ba1-af16-c409d1a7db9b')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (14, N'196ff048-cdc3-4c7e-a411-87feaa249ac5', N'54a56dc2-4e86-485b-9ee5-a780d95e0927')
INSERT [dbo].[RegionalManagerRestaurantIds] ([Id], [RestaurantId], [RegionalManagerId]) VALUES (15, N'520dc622-9921-4337-b0b9-d7bd271a76a0', N'87f810dc-81df-4969-92a3-c18d0a74d8b5')
SET IDENTITY_INSERT [dbo].[RegionalManagerRestaurantIds] OFF
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'b5ba9e2d-7a9f-4d14-97e8-3b85aade4dce', N'f4968894-0b55-40c4-aed4-7fc1762b8195')
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'87f810dc-81df-4969-92a3-c18d0a74d8b5', N'40b7bd33-6e61-4156-8a6b-852c8411de5d')
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'f3848a49-1d62-4bc9-8b9b-549a3dbd3475', N'f59cf698-6f65-4902-8593-87e790931cbf')
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'196bd483-0f8e-4ba1-af16-c409d1a7db9b', N'179564f7-fe5a-4e48-b007-8ca7362d2b46')
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'54a56dc2-4e86-485b-9ee5-a780d95e0927', N'7a620a50-e602-4dd4-aaa1-b9c176369ec3')
INSERT [dbo].[RegionalManagers] ([RegionalManagerId], [UserId]) VALUES (N'0623bc31-8f01-4e69-b3df-e2a651c97479', N'fd30e861-a84d-43aa-b90c-c7ad8ae07104')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'00e062e2-57d4-4b22-a6f4-009b785f6583', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'd7163a81-3605-4036-9e56-0ad014c7b9fc', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'a89332ec-feda-44bc-bc09-223a600f6398', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'dd42de9d-7fe7-4540-a869-22ae4a402223', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'8b95345e-269e-4ca2-8012-3163b7dc0237', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'd46425a6-be79-48ea-8d52-3baf851b6dcf', N'gfg', N'fg', N'fg', N'fg')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'54de692c-287b-4944-910e-42a9fa8a475a', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'b35ff74e-e94c-4fc7-9a9e-63d9c7f0722d', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'd8e4e33e-1b62-4ccb-9eb0-6efe7e0f0161', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'c3f95301-9d15-4c27-967f-6f791d304653', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'c6e7d4c2-b0f0-4709-9d3a-745ece4fbd7d', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'8f11efa1-875b-489c-8a30-7e4414369f9a', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'5761104b-44f7-439c-9ce8-7edad54baef4', N'dsa', N'dsds', N'dsds', N'dsds')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'196ff048-cdc3-4c7e-a411-87feaa249ac5', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'a2e4666f-95d1-4eb5-9079-8950bc1fd8ef', N'test street', N'test city', N'test code', N'test city')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'845bf26a-97d5-4ef6-ab67-9734019494d5', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'b073d450-d585-4ab0-a908-9edf1e8e001b', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'f5e01790-d313-4b65-b42d-a01a1a21be1f', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'1bdccf30-8cc1-441b-bb32-ab3f40da1df0', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'46671117-1955-41ae-a254-bbded21a1fcf', N'dd', N'dd', N'dd', N'dd')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'a2c10981-7df3-4ec4-b1dd-c03766966f4a', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'56ed1931-58cd-4215-a207-c47612868ecc', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'c69e00e3-03d5-4564-aa52-ceb3ef29694b', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'83b1b437-87c0-4430-ba8f-d30d466cc478', N'fff', N'fff', N'fff', N'fff')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'd49c8a4d-7be5-4fb1-bd6d-d617f1f0160c', N'pppp', N'pppp', N'pppp', N'pppp')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'520dc622-9921-4337-b0b9-d7bd271a76a0', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'2a9fb8d1-313f-41b2-9688-e0aadbbd1a90', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Restaurants] ([Id], [Address_Street], [Address_City], [Address_PostalCode], [Address_Country]) VALUES (N'79f86506-305a-4827-b1a4-edf786cef5e6', N'123 Main Street', N'Anytown', N'12345', N'USA')
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'c41f23c1-31d0-4460-ab4c-1ac56c9967b8', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Standard', N'Test title2', CAST(N'2012-04-23T18:25:43.5110000' AS DateTime2), CAST(N'2012-04-23T18:25:43.5110000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'1898632a-eaa0-46e5-a289-26442b2183f1', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Essential', N'test1234', CAST(N'2023-12-24T23:37:37.0000000' AS DateTime2), CAST(N'2023-12-24T23:37:37.0000000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'ca88f9ef-1966-43f8-92a8-3cad1723a7a1', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Standard', N'fdgg', CAST(N'2023-12-24T22:19:01.0720000' AS DateTime2), CAST(N'2023-12-24T22:19:01.0720000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'b48d1430-2df5-4146-840e-949dd855da0a', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Standard', N'gdfg', CAST(N'2023-12-30T23:00:00.0000000' AS DateTime2), CAST(N'2023-12-31T22:59:59.9990000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'b381deaf-1b31-4f3a-aa65-b46fcd1e7c15', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Standard', N'utc', CAST(N'2023-12-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-12-04T23:59:59.0000000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'9d2f1a3a-53bc-4a1c-8313-b505abd63263', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Standard', N'teeeeeee', CAST(N'1905-12-02T23:00:00.0000000' AS DateTime2), CAST(N'1906-12-03T22:59:59.0000000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'c9653cb0-e2bd-4367-a2e0-b9570c35a6ac', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Essential', N'wyjebane na robotę', CAST(N'2023-12-18T23:00:00.0000000' AS DateTime2), CAST(N'2023-12-19T22:59:59.9990000' AS DateTime2))
INSERT [dbo].[Schedules] ([ScheduleId], [RestaurantId], [Priority], [Title], [StartDate], [EndDate]) VALUES (N'b045a695-0821-45e7-99fb-e6d5410fcdfb', N'e2e115cf-4a20-40e4-add5-67cf34788a0a', N'Essential', N'Test title_newName', CAST(N'2023-12-20T00:00:00.0000000' AS DateTime2), CAST(N'2023-12-22T23:59:59.0000000' AS DateTime2))
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'8683621d-5db1-4458-987f-0507408ec3ee', N'3f2r2U+/pGcqw2fXwVkG1erdIltqgC1JJwizv9/113Y6Q9DIn10nOzWCC7IvzAOPbVOFjMERnLa6FSRaLMMePg==.U97cBJbhSC/OEYpDNdF/Ik5IIhwIVlLPl9Ao7Olrf2IoVhfrT3gK+06Rnp1blDBDyljZp1gxdmanUoLplXnkkQ==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'bd966720-7a2e-44bb-9698-2b574d175713', N't9MXwHK06aYX4v2Ir/CDct16QQGhSboApdW8wJgxWLdAiT7YODfeG94ZVcg+OUFLZUbhtgVB6jfjaVXv+KC+TQ==.15F5IsjqMIqONWhTzcedCnDP2DE53mOTNV+1+D/WcREkm6lJts0SVbG8M4Kz44PVHaSWfINriwBSzvOZjuFAxQ==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'd025a95e-6b84-40c8-ad63-4f0f59a250d2', N'dpBoB03zfdyTVa8Z6kH/Y5/JuaHi0M62vRs3EFG/xFHQEwfSep7+4cc5DptAHG4gnMfNI98mu7PZ11O4tmwarQ==.POa4xCudabdKxWGoiu1q68AuEgxRM8MJn8AoFbnRzX2u5pqoG80/vMQtduWpIpnCrEH+g5tykP59CzjeCPqFLg==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'e7a4344b-6141-422d-9f61-5421958ed8b4', N'uJH4mdpo8c3IvPibx3kaDA5FjPTYE1m4pNP8cdLqGsg3VZvFlP4WKR9CZ5rmZh0UKtdZfz1vYAdaUmLLcfHYWA==.dn5HDxYhMaNfA/YFfZzliI/IEQXxBEl5CE4fCD0ksLbfURcHbF9GMtVhO4po0Cr6HUfUfdrSW/17Ae9uRUgwmA==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'c2a63572-66b9-41bb-a8a7-68c72ee14724', N'GTnaEHzI55sfsdmqsMZUZ1GxvgH6BYYv7/DmTA7RNn270/9+Q9zBnXjttYVk/g8JOvMaHs0O0XUU1/qvj3al7g==.Z2yumDOS2t8aHZLKJXjZfZCBhsuSFyt7xcGyzTryEgEix7yW4qQZkOoQFAqZKkMQ43SruiQuZso6eOD/sr+org==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'1b9ec2e8-b776-4bc1-8faa-7ab1ceebe531', N'HMtVpchGWNAJtmA1+s4lavlcfm9pMxWpU0A4fjK3a51F40Iig9o+GnZGyK+K95spdQvqDP63jZ6xh6wt2+sU0w==.qxDC6D4DfN9fwD7j/5Dty0y0WASS6TOhAOJ2Z5SfZ6XRhKNgTKk+5sPl+Y44v7oSK0qofjGT13krPGeSd0E05w==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'f4968894-0b55-40c4-aed4-7fc1762b8195', N'S1pu/g5uvWfYEEKNcdHHRbE7RvfBy3ddzbuAQLwPxNN21aMhZzCAryONW+rpSGnaoG6HwCc3+qtyXvPVOaxFow==./tind4ICVtxPVdEIdZcIqt5w9w5aWgTmH14eb2szul5IRIe0jGEJROxC6aLoViBBDSfykyQ12FCqh/quh+iwaA==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'40b7bd33-6e61-4156-8a6b-852c8411de5d', N'TDwLaoKXG5fD+Aw25t0SwrZUtqZ0tYQQNWvzGv3/9Aqhr7rM0JExeTTQ6OsKrB3arlOGp1qJPdQSAmAZr7t6Pw==.8h6U0GSxfgknit28d/qz3FHOQsteIN6uPVPcJHVlPIXxlq374SiMrt27L6AL6BYSL7U7US800a08XSpt21AG0Q==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'f59cf698-6f65-4902-8593-87e790931cbf', N'3f2r2U+/pGcqw2fXwVkG1erdIltqgC1JJwizv9/113Y6Q9DIn10nOzWCC7IvzAOPbVOFjMERnLa6FSRaLMMePg==.U97cBJbhSC/OEYpDNdF/Ik5IIhwIVlLPl9Ao7Olrf2IoVhfrT3gK+06Rnp1blDBDyljZp1gxdmanUoLplXnkkQ==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'179564f7-fe5a-4e48-b007-8ca7362d2b46', N'wUmaeDIItesh17BSjf6iVBzbaglr581OP5nJzphVP36/68V2nqtl682zb47Lqla5bMkvlBCtdYmeG9FYok+E8Q==.ugqGsNYqUdzmjSPlwmLAJpgMzIDb6lAzvVK7lNo51/ydVr1EKjhu0VUJfUxYwzVvf+0O2Vn7FrHf6Tz+uqkqUA==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'f0658c94-aab2-41c4-b3ac-9445f8c72da6', N'kIilTekWWnP7/H+2rx1KvbbvkvoPmoAETAo3l6LQ4w6Qyp7BPxpjCuJC/VxdRDKTFGm9nbJC+oxuliabIf5jcg==.mie/3UKSQ5Bh+d2UWzK8rLtlLWDaSwea2zdfs8+aCD9v85DLYQ2YWmatqB8goRRt5kAlNNuhYq5W/3tG23+U5A==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'5450da1b-4622-4f0e-b7d1-a6f0c3b82bf4', N'BvudAZz1Hq3/Dj4df4tkPU5coZ+2VLz3HIPrVdb60c4p3co7fNObpz+O9mAk0nJd6MU/AZkeUa+fq0HDZGilxw==.SrTsza77DJytgS7PtwvpbTVgABZ5jFUj9YV8THTnuDdEXyd7GodI5GeVOzAR4sLLyvKyOSaTfF30xxh+ChhHQw==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'7a620a50-e602-4dd4-aaa1-b9c176369ec3', N'0SiA0AsrGtuTG0PMt/2LeZP+KiM2SdmA6e2YmnnVR7kmb32+jLuCcgboBOrN4InHTa8FyinXi1Tg08bIieTjPg==.aarEHh8z4J1UoA2tgPagt0jaoHBl94ZFKp/10mb+1opKl6Ye2PdYf+4QoeZKB8DkelSQc+sUk912znCWg8lLUA==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'1eda9289-413c-4c79-b764-c62552b0b5ea', N'OxvsjdUXvcIQnRDYaxxaKU4SvUvPFcErc9Xg4hHA3Z2nbeorSBz4/aflWPWLFxd52uvQ3+TosZkFfv4gKh1t8g==.frLOXKjD16hKeJzZw1JKlPQE6/AfquJIYApDXVL+Zd6E1nZVCrbdNoMpxvkX+aM2CElkC+Rz92BpvszL/Wxt1Q==')
INSERT [dbo].[UserPasswords] ([UserId], [Password]) VALUES (N'fd30e861-a84d-43aa-b90c-c7ad8ae07104', N'O0ydnOwktVto7Ug7Aqfzg886R9JHlSngwu3XWexSFzeNcLge/01778zO2arLVzUocuDYunIy9+j2WZMb/aoHhg==.B6284Q8zqXjB3NBtumV6LLlRG1VN9lO9DhKPPgZUrvDvpPMd5Oiu/yve3qvVOpRtq/bAlFrHOU+psjaQ1BJ8lA==')
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'8683621d-5db1-4458-987f-0507408ec3ee', N'John', N'Johnson', N'john1@john.com', N'Manager', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'bd966720-7a2e-44bb-9698-2b574d175713', N'journal', N'user', N'journal@test.com', N'Manager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'd025a95e-6b84-40c8-ad63-4f0f59a250d2', N'dert', N'trerte', N'hkjhgkhj@dsd.bb', N'Manager', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'e7a4344b-6141-422d-9f61-5421958ed8b4', N'Donald', N'Trump', N'johnManager@john.com', N'Courier', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'c2a63572-66b9-41bb-a8a7-68c72ee14724', N'fsdfsd', N'sdfdsdsf', N'fffff@ffff.com', N'Manager', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'1b9ec2e8-b776-4bc1-8faa-7ab1ceebe531', N'rerte', N'gfdweerw', N'jhjkh@retre.com', N'Manager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'f4968894-0b55-40c4-aed4-7fc1762b8195', N'ddd', N'ddd', N'test@test123.com', N'RegionalManager', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'40b7bd33-6e61-4156-8a6b-852c8411de5d', N'gfdfdg', N'gdfgd', N'test123@tttt.com', N'RegionalManager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'f59cf698-6f65-4902-8593-87e790931cbf', N'John', N'Johnson', N'john@john.com', N'GlobalManager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'179564f7-fe5a-4e48-b007-8ca7362d2b46', N'fdfdt', N'tretre', N'fgfggfgff@uyyuyu.com', N'RegionalManager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'f0658c94-aab2-41c4-b3ac-9445f8c72da6', N'firstn', N'sndn', N'courier@test.com', N'Courier', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'5450da1b-4622-4f0e-b7d1-a6f0c3b82bf4', N'konto', N'nazwisko', N'konto@email.com', N'Manager', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'7a620a50-e602-4dd4-aaa1-b9c176369ec3', N'dsffsd', N'dsffds', N'gdffgd@rr.sdfds', N'RegionalManager', 1)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'1eda9289-413c-4c79-b764-c62552b0b5ea', N'wrewr', N'rtetre', N'kkkkkk@qqqq.mm', N'Manager', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [UserType], [IsEmailConfirmed]) VALUES (N'fd30e861-a84d-43aa-b90c-c7ad8ae07104', N'fsdffds', N'hfgghf', N'fgsdfsad@fd.vom', N'RegionalManager', 0)
/****** Object:  Index [IX_Couriers_RegionalManagerId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Couriers_RegionalManagerId] ON [dbo].[Couriers]
(
	[RegionalManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Couriers_UserId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Couriers_UserId] ON [dbo].[Couriers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Employees_RestaurantId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Employees_RestaurantId] ON [dbo].[Employees]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmployeesSchedules_ScheduleId_ScheduleRestaurantId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_EmployeesSchedules_ScheduleId_ScheduleRestaurantId] ON [dbo].[EmployeesSchedules]
(
	[ScheduleId] ASC,
	[ScheduleRestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ingredients_MenuItemId_MenuItemMenuId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Ingredients_MenuItemId_MenuItemMenuId] ON [dbo].[Ingredients]
(
	[MenuItemId] ASC,
	[MenuItemMenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Managers_RestaurantId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Managers_RestaurantId] ON [dbo].[Managers]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Managers_UserId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Managers_UserId] ON [dbo].[Managers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MenuItems_MenuId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_MenuItems_MenuId] ON [dbo].[MenuItems]
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_OrderTime_IsCancelled]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_OrderTime_IsCancelled] ON [dbo].[Orders]
(
	[OrderTime] DESC,
	[IsCancelled] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_RestaurantId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_RestaurantId] ON [dbo].[Orders]
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Packages_DestinationRestaurant]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Packages_DestinationRestaurant] ON [dbo].[Packages]
(
	[DestinationRestaurant] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Packages_RegionalManager]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_Packages_RegionalManager] ON [dbo].[Packages]
(
	[RegionalManager] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RegionalManagerRestaurantIds_RegionalManagerId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_RegionalManagerRestaurantIds_RegionalManagerId] ON [dbo].[RegionalManagerRestaurantIds]
(
	[RegionalManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RegionalManagers_UserId]    Script Date: 29.01.2024 00:18:56 ******/
CREATE NONCLUSTERED INDEX [IX_RegionalManagers_UserId] ON [dbo].[RegionalManagers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Schedules_RestaurantId]    Script Date: 29.01.2024 00:18:56 ******/
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