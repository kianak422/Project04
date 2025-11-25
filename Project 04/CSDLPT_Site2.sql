USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CSDLPT_Site2')
BEGIN
    CREATE DATABASE [CSDLPT_Site2];
END;
GO