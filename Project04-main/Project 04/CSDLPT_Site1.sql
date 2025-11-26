USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CSDLPT_Site1')
BEGIN
    CREATE DATABASE [CSDLPT_Site1];
END;
GO