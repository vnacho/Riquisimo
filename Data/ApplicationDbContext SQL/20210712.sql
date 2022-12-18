ALTER TABLE [Personal] ADD [PrecioHora] decimal(18,2) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210712123329_preciohora', N'3.1.2');

GO

