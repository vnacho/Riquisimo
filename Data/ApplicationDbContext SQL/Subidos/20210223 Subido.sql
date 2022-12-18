ALTER TABLE [Accounts] ADD [Operario] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210223181834_operarioAccount', N'3.1.2');

GO