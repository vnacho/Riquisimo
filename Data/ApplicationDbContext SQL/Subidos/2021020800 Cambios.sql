ALTER TABLE [CompraPedidos] ADD [CodigoEvento] nvarchar(max) NOT NULL DEFAULT N'';

GO

ALTER TABLE [CompraPedidos] ADD [NombreEvento] nvarchar(max) NULL;

GO

ALTER TABLE [CompraFacturas] ADD [CodigoEvento] nvarchar(max) NOT NULL DEFAULT N'';

GO

ALTER TABLE [CompraFacturas] ADD [NombreEvento] nvarchar(max) NULL;

GO

ALTER TABLE [CompraAlbaranes] ADD [CodigoEvento] nvarchar(max) NOT NULL DEFAULT N'';

GO

ALTER TABLE [CompraAlbaranes] ADD [NombreEvento] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210208155550_eventoCabecera', N'3.1.2');

GO

