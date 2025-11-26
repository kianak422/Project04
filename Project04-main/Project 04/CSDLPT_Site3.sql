USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CSDLPT_Site3')
BEGIN
    CREATE DATABASE [CSDLPT_Site3];
END;
GO