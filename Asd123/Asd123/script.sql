IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171104230011_InitialCreate')
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [Email] nvarchar(max) NULL,
        [Name] nvarchar(max) NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [UserIdentifier] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171104230011_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20171104230011_InitialCreate', N'2.0.1-rtm-125');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111140750_AddImageInfo')
BEGIN
    CREATE TABLE [ImageInfos] (
        [Id] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [ImageId] nvarchar(max) NULL,
        [ImageUri] nvarchar(max) NULL,
        [Name] nvarchar(max) NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [UploadedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_ImageInfos] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111140750_AddImageInfo')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20171111140750_AddImageInfo', N'2.0.1-rtm-125');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111202653_AddRelationships')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'ImageInfos') AND [c].[name] = N'UploadedBy');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ImageInfos] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [ImageInfos] DROP COLUMN [UploadedBy];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111202653_AddRelationships')
BEGIN
    ALTER TABLE [ImageInfos] ADD [UploadedById] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111202653_AddRelationships')
BEGIN
    CREATE INDEX [IX_ImageInfos_UploadedById] ON [ImageInfos] ([UploadedById]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111202653_AddRelationships')
BEGIN
    ALTER TABLE [ImageInfos] ADD CONSTRAINT [FK_ImageInfos_Users_UploadedById] FOREIGN KEY ([UploadedById]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171111202653_AddRelationships')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20171111202653_AddRelationships', N'2.0.1-rtm-125');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171112182449_user')
BEGIN
    ALTER TABLE [Users] ADD [DateOfBirth] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171112182449_user')
BEGIN
    ALTER TABLE [Users] ADD [Gender] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171112182449_user')
BEGIN
    ALTER TABLE [Users] ADD [Hometown] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171112182449_user')
BEGIN
    ALTER TABLE [Users] ADD [Locale] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171112182449_user')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20171112182449_user', N'2.0.1-rtm-125');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171113215521_Tags')
BEGIN
    CREATE TABLE [Tag] (
        [Id] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Tag] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171113215521_Tags')
BEGIN
    CREATE TABLE [PictureTag] (
        [Id] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [ImageId] uniqueidentifier NULL,
        [TagId] uniqueidentifier NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        CONSTRAINT [PK_PictureTag] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PictureTag_ImageInfos_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [ImageInfos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PictureTag_Tag_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tag] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171113215521_Tags')
BEGIN
    CREATE INDEX [IX_PictureTag_ImageId] ON [PictureTag] ([ImageId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171113215521_Tags')
BEGIN
    CREATE INDEX [IX_PictureTag_TagId] ON [PictureTag] ([TagId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20171113215521_Tags')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20171113215521_Tags', N'2.0.1-rtm-125');
END;

GO

