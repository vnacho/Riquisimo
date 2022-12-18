ALTER TABLE [Congresses] ADD [SwiftCode] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210317095827_swift', N'3.1.2');

GO

