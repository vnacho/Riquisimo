--¡¡¡Atención!!! las antiguas facturas sería mejor
--1 cambiarles el estadoa no traspasadas desde la base de datos (aquellas que estén traspasadas, apuntarlas)
--2 luego guardarlas desde la aplicación para que calculen de nuevo el total
--3 cambiarles el estadoa a traspasadas desde la base de datos

ALTER TABLE [CompraFacturas] ADD [BaseImponible] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210414155720_total', N'3.1.2');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturaLineas]') AND [c].[name] = N'IVA_Porcentaje');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturaLineas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CompraFacturaLineas] ALTER COLUMN [IVA_Porcentaje] int NULL;

GO

ALTER TABLE [Accounts] ADD [AccessCompras] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210416094411_accesocompras', N'3.1.2');

GO

