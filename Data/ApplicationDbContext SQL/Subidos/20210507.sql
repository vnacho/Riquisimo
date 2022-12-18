DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturas]') AND [c].[name] = N'Retencion_Porcentaje');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CompraFacturas] ALTER COLUMN [Retencion_Porcentaje] decimal(18,2) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210507101427_retencion', N'3.1.2');

GO

