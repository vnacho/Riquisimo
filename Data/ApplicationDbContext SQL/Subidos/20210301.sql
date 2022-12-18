--¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡LO PRIMERO BORRAR FACTURAS, ALBARANES Y PEDIDOS!!!!!!!!!!!!!!!!!!!

--¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡Aplicar appsettings.js nuevos valores!!!!!!!!!!!!!!!!!!!!!!!!!!!!

ALTER TABLE [CompraFacturaLineas] ADD [CodigoTipoIVA] nvarchar(max) NOT NULL DEFAULT N'';

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210301092153_codigotipoiva', N'3.1.2');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturas]') AND [c].[name] = N'NumeroFactura');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CompraFacturas] ALTER COLUMN [NumeroFactura] nvarchar(10) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturas]') AND [c].[name] = N'CodigoSage');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [CompraFacturas] ALTER COLUMN [CodigoSage] int NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210302140115_codigosage', N'3.1.2');

GO

