DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personal]') AND [c].[name] = N'IBAN');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Personal] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Personal] ALTER COLUMN [IBAN] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210702073245_iban', N'3.1.2');

GO

