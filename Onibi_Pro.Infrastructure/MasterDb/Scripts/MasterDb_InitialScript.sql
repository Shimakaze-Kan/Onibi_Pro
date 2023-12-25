/****************************************************************************
*	CHANGELOG
*	WHO		        WHEN		WHAT			
*	Shimakaze-Kan	12/25/2023	Initial script	
*
*****************************************************************************/

USE master;
GO

-- Check if the database exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Onibi_MasterDb')
BEGIN
    -- Create the database
    CREATE DATABASE Onibi_MasterDb;
END
GO

-- Use the "Onibi_MasterDb" database
USE Onibi_MasterDb;
GO

-- Check if the "OnibiClients" table exists
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OnibiClients')
BEGIN
    -- Create the "OnibiClients" table
    CREATE TABLE OnibiClients (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(255) NOT NULL,
        CONSTRAINT UQ_OnibiClients_Name UNIQUE (Name)
    );
END
GO

-- Check if the "OnibiUsers" table exists
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OnibiUsers')
BEGIN
    -- Create the "OnibiUsers" table
    CREATE TABLE OnibiUsers (
        Email NVARCHAR(255) PRIMARY KEY,
        ClientId INT,
        CONSTRAINT UQ_OnibiUsers_Email UNIQUE (Email),
        FOREIGN KEY (ClientId) REFERENCES OnibiClients(Id)
    );
END
GO

-- Check if the "AddUser" procedure exists
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'AddUser')
BEGIN
    -- Create the "AddUser" procedure
    EXEC('
    CREATE PROCEDURE AddUser
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
            THROW 50000, ''Client does not exist.'', 1;
        END;

        -- Add the user
        INSERT INTO OnibiUsers (Email, ClientId) VALUES (@Email, @ClientId);
    END;
    ');
END
GO

-- Check if the "AddClient" procedure exists
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'AddClient')
BEGIN
    -- Create the "AddClient" procedure
    EXEC('
    CREATE PROCEDURE AddClient
        @ClientName NVARCHAR(255)
    AS
    BEGIN
        -- Add the client
        INSERT INTO OnibiClients (Name) VALUES (@ClientName);
    END;
    ');
END
GO

-- Check if the "GetClientNameByEmail" procedure exists
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'GetClientNameByEmail')
BEGIN
    -- Create the "GetClientNameByEmail" procedure
    EXEC('
    CREATE PROCEDURE GetClientNameByEmail
        @Email NVARCHAR(255),
        @ClientName NVARCHAR(255) OUTPUT
    AS
    BEGIN
        -- Get the client name based on the user''s email
        SELECT @ClientName = c.Name
        FROM OnibiUsers u
        JOIN OnibiClients c ON u.ClientId = c.Id
        WHERE u.Email = @Email;
    END;
    ');
END
GO
