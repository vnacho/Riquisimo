DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personal]') AND [c].[name] = N'IBAN');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Personal] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Personal] ALTER COLUMN [IBAN] nvarchar(4) NULL;

GO

ALTER TABLE [Personal] ADD [ObraId] int NULL;

GO

CREATE INDEX [IX_Personal_ObraId] ON [Personal] ([ObraId]);

GO

ALTER TABLE [Personal] ADD CONSTRAINT [FK_Personal_CentrosCoste_ObraId] FOREIGN KEY ([ObraId]) REFERENCES [CentrosCoste] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210630102606_obra', N'3.1.2');

GO

