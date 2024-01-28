USE [master]
GO
/****** Object:  Database [Onibi_MasterDb]    Script Date: 29.01.2024 00:17:34 ******/
CREATE DATABASE [Onibi_MasterDb]
 CONTAINMENT = NONE
GO
ALTER DATABASE [Onibi_MasterDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Onibi_MasterDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Onibi_MasterDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Onibi_MasterDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Onibi_MasterDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Onibi_MasterDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Onibi_MasterDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET RECOVERY FULL 
GO
ALTER DATABASE [Onibi_MasterDb] SET  MULTI_USER 
GO
ALTER DATABASE [Onibi_MasterDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Onibi_MasterDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Onibi_MasterDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Onibi_MasterDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Onibi_MasterDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Onibi_MasterDb', N'ON'
GO
ALTER DATABASE [Onibi_MasterDb] SET QUERY_STORE = OFF
GO
USE [Onibi_MasterDb]
GO
/****** Object:  Table [dbo].[OnibiClients]    Script Date: 29.01.2024 00:17:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnibiClients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OnibiUsers]    Script Date: 29.01.2024 00:17:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnibiUsers](
	[Email] [nvarchar](255) NOT NULL,
	[ClientId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[OnibiClients] ON 

INSERT [dbo].[OnibiClients] ([Id], [Name]) VALUES (1, N'McDowel')
SET IDENTITY_INSERT [dbo].[OnibiClients] OFF
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'courier@test.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'dupa@dupa', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'fffff@ffff.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'fgfggfgff@uyyuyu.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'fghgf@fgfdg.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'fgsdfsad@fd.vo', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'fgsdfsad@fd.vom', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'gdffgd@rr.sdfds', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'hkjhgkhj@dsd.bb', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'jhjh@dsd.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'jhjkh@retre.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'john@john.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'johnManager@john.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'journal@test.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'kjkkjl@jhgfjh.pl', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'kkkkkk@qqqq.mm', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'konto@email.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'llllll@www.jj', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'multipleClients@john.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'test@test123.com', 1)
INSERT [dbo].[OnibiUsers] ([Email], [ClientId]) VALUES (N'test123@tttt.com', 1)
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_OnibiClients_Name]    Script Date: 29.01.2024 00:17:35 ******/
ALTER TABLE [dbo].[OnibiClients] ADD  CONSTRAINT [UQ_OnibiClients_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_OnibiUsers_Email]    Script Date: 29.01.2024 00:17:35 ******/
ALTER TABLE [dbo].[OnibiUsers] ADD  CONSTRAINT [UQ_OnibiUsers_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OnibiUsers]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[OnibiClients] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[AddClient]    Script Date: 29.01.2024 00:17:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[AddClient]
        @ClientName NVARCHAR(255)
    AS
    BEGIN
        -- Add the client
        INSERT INTO OnibiClients (Name) VALUES (@ClientName);
    END;
    
GO
/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 29.01.2024 00:17:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[AddUser]
        @Email NVARCHAR(255),
        @ClientName NVARCHAR(255)
    AS
    BEGIN
        DECLARE @ClientId INT;

        -- Check if the client exists
        SELECT @ClientId = Id FROM OnibiClients WHERE Name = @ClientName;

        IF @ClientId IS NULL
        BEGIN
            -- If the client does not exist, raise an error
            THROW 50000, 'Client does not exist.', 1;
        END;

        -- Add the user
        INSERT INTO OnibiUsers (Email, ClientId) VALUES (@Email, @ClientId);
    END;
    
GO
/****** Object:  StoredProcedure [dbo].[GetClientNameByEmail]    Script Date: 29.01.2024 00:17:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[GetClientNameByEmail]
        @Email NVARCHAR(255),
        @ClientName NVARCHAR(255) OUTPUT
    AS
    BEGIN
        -- Get the client name based on the user's email
        SELECT @ClientName = c.Name
        FROM OnibiUsers u
        JOIN OnibiClients c ON u.ClientId = c.Id
        WHERE u.Email = @Email;
    END;
    
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 29.01.2024 00:17:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE PROCEDURE [dbo].[UpdateUser]
        @OldEmail NVARCHAR(255),
        @NewEmail NVARCHAR(255),
		@ClientName NVARCHAR(255)
    AS
    BEGIN
		DECLARE @ClientId INT;

        -- Check if the client exists
        SELECT @ClientId = Id FROM OnibiClients WHERE Name = @ClientName;

        IF @ClientId IS NULL
        BEGIN
            -- If the client does not exist, raise an error
            THROW 50000, 'Client does not exist.', 1;
        END;

        -- Update the users email
        UPDATE OnibiUsers SET Email = @NewEmail WHERE Email = @OldEmail AND ClientId = @ClientId;
    END;
    
GO
USE [master]
GO
ALTER DATABASE [Onibi_MasterDb] SET  READ_WRITE 
GO
