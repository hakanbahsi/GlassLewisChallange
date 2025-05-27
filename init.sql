IF DB_ID(N'GlassLewisDb') IS NULL
BEGIN
    CREATE DATABASE GlassLewisDb;
END
GO

USE GlassLewisDb;
GO

IF OBJECT_ID(N'[__MigrationsHistoryOfGlassLewis]') IS NULL
BEGIN
    CREATE TABLE [__MigrationsHistoryOfGlassLewis] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___MigrationsHistoryOfGlassLewis] PRIMARY KEY ([MigrationId])
    );
END
GO

IF OBJECT_ID(N'[COMPANIES]') IS NULL
BEGIN
    CREATE TABLE [COMPANIES] (
        [ID] uniqueidentifier NOT NULL,
        [NAME] nvarchar(300) NOT NULL,
        [EXCHANGE] nvarchar(100) NOT NULL,
        [TICKER] nvarchar(50) NOT NULL,
        [ISIN] nvarchar(20) NOT NULL,
        [WEBSITE] nvarchar(600) NULL,
        CONSTRAINT [PK_COMPANIES] PRIMARY KEY ([ID])
    );

    CREATE INDEX [IX_COMPANIES_ISIN] ON [COMPANIES] ([ISIN]);

    INSERT INTO [__MigrationsHistoryOfGlassLewis] ([MigrationId], [ProductVersion])
    VALUES (N'20250526235748_FirstMigration', N'9.0.5');
END
GO
