--Atención hay que modificar el appsetting la cadena de conexión de sage para obtenerla dinámicamente => 
--"Sage": "Server=JAVI-PC\\SQLSERVER2019;Database=SAGE{0};Trusted_Connection=True;MultipleActiveResultSets=true;"

CREATE TABLE [CompraPedidos] (
    [Id] int NOT NULL IDENTITY,
    [CodigoPedido] nvarchar(max) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Observaciones] nvarchar(max) NULL,
    [Total] decimal(18,2) NOT NULL,
    [CodigoProveedor] nvarchar(max) NOT NULL,
    [NombreProveedor] nvarchar(max) NOT NULL,
    [CodigoOperario] nvarchar(max) NOT NULL,
    [NombreOperario] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CompraPedidos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CompraPedidoLineas] (
    [IdPedidoLinea] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [CodigoArticulo] nvarchar(max) NOT NULL,
    [NombreArticulo] nvarchar(max) NULL,
    [ObservacionesPedidoLinea] nvarchar(max) NULL,
    [CodigoEvento] nvarchar(max) NOT NULL,
    [NombreEvento] nvarchar(max) NULL,
    [Unidades] decimal(18,2) NOT NULL,
    [UnidadesPendientes] decimal(18,2) NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [TotalPedidoLinea] decimal(18,2) NOT NULL,
    [Orden] int NOT NULL,
    CONSTRAINT [PK_CompraPedidoLineas] PRIMARY KEY ([IdPedidoLinea]),
    CONSTRAINT [FK_CompraPedidoLineas_CompraPedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [CompraPedidos] ([Id]) ON DELETE CASCADE
);

GO

ALTER TABLE [CompraPedidos] ADD [EstadoPedido] int NOT NULL DEFAULT 0;

GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-05T18:34:02.7496367+01:00', [Modified] = '2021-01-05T18:34:02.7496382+01:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-05T18:34:02.7495203+01:00', [Modified] = '2021-01-05T18:34:02.7495767+01:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-05T18:34:02.7496402+01:00', [Modified] = '2021-01-05T18:34:02.7496405+01:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210105173403_estadoPedido', N'3.1.2');

GO


CREATE TABLE [CompraAlbaranes] (
    [Id] int NOT NULL IDENTITY,
    [CodigoAlbaran] nvarchar(max) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Observaciones] nvarchar(max) NULL,
    [Total] decimal(18,2) NOT NULL,
    [CodigoProveedor] nvarchar(max) NOT NULL,
    [NombreProveedor] nvarchar(max) NOT NULL,
    [CodigoOperario] nvarchar(max) NOT NULL,
    [NombreOperario] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CompraAlbaranes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CompraAlbaranLineas] (
    [IdAlbaranLinea] int NOT NULL IDENTITY,
    [AlbaranId] int NOT NULL,
    [CodigoArticulo] nvarchar(max) NOT NULL,
    [NombreArticulo] nvarchar(max) NULL,
    [ObservacionesAlbaranLinea] nvarchar(max) NULL,
    [CodigoEvento] nvarchar(max) NOT NULL,
    [NombreEvento] nvarchar(max) NULL,
    [Unidades] decimal(18,2) NOT NULL,
    [UnidadesPendientes] decimal(18,2) NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [TotalAlbaranLinea] decimal(18,2) NOT NULL,
    [Orden] int NOT NULL,
    CONSTRAINT [PK_CompraAlbaranLineas] PRIMARY KEY ([IdAlbaranLinea]),
    CONSTRAINT [FK_CompraAlbaranLineas_CompraAlbaranes_AlbaranId] FOREIGN KEY ([AlbaranId]) REFERENCES [CompraAlbaranes] ([Id]) ON DELETE CASCADE
);

GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:22:08.7989837+01:00', [Modified] = '2021-01-07T11:22:08.7989848+01:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:22:08.7988685+01:00', [Modified] = '2021-01-07T11:22:08.7989251+01:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:22:08.7989866+01:00', [Modified] = '2021-01-07T11:22:08.7989870+01:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

CREATE INDEX [IX_CompraAlbaranLineas_AlbaranId] ON [CompraAlbaranLineas] ([AlbaranId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210107102209_compraAlbaranes', N'3.1.2');

GO


DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraAlbaranLineas]') AND [c].[name] = N'UnidadesPendientes');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompraAlbaranLineas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CompraAlbaranLineas] DROP COLUMN [UnidadesPendientes];

GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:46:33.6210058+01:00', [Modified] = '2021-01-07T11:46:33.6210068+01:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:46:33.6208896+01:00', [Modified] = '2021-01-07T11:46:33.6209457+01:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:46:33.6210088+01:00', [Modified] = '2021-01-07T11:46:33.6210091+01:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210107104634_quitarUnidadesPendientesAlbaranLinea', N'3.1.2');

GO


ALTER TABLE [CompraAlbaranes] ADD [EstadoAlbaran] int NOT NULL DEFAULT 0;

GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:57:23.7528749+01:00', [Modified] = '2021-01-07T11:57:23.7528760+01:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:57:23.7527274+01:00', [Modified] = '2021-01-07T11:57:23.7527845+01:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2021-01-07T11:57:23.7528781+01:00', [Modified] = '2021-01-07T11:57:23.7528784+01:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210107105724_estadoAlbaran', N'3.1.2');

GO

ALTER TABLE [CompraAlbaranLineas] ADD [CompraPedidoLineaId] int NULL;

GO

CREATE INDEX [IX_CompraAlbaranLineas_CompraPedidoLineaId] ON [CompraAlbaranLineas] ([CompraPedidoLineaId]);

GO

ALTER TABLE [CompraAlbaranLineas] ADD CONSTRAINT [FK_CompraAlbaranLineas_CompraPedidoLineas_CompraPedidoLineaId] FOREIGN KEY ([CompraPedidoLineaId]) REFERENCES [CompraPedidoLineas] ([IdPedidoLinea]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210107120847_RelacionLineasAlbaranPedido', N'3.1.2');

GO

ALTER TABLE [CompraAlbaranes] ADD [CompraFacturaId] int NULL;

GO

ALTER TABLE [CompraAlbaranes] ADD [GeneradoAutomaticamente] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

CREATE TABLE [CompraFacturas] (
    [Id] int NOT NULL IDENTITY,
    [CodigoSage] nvarchar(max) NULL,
    [NumeroFactura] nvarchar(max) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Observaciones] nvarchar(max) NULL,
    [Total] decimal(18,2) NOT NULL,
    [CodigoProveedor] nvarchar(max) NOT NULL,
    [NombreProveedor] nvarchar(max) NOT NULL,
    [CodigoOperario] nvarchar(max) NOT NULL,
    [NombreOperario] nvarchar(max) NOT NULL,
    [EstadoFactura] int NOT NULL,
    [TieneFichero] bit NOT NULL,
    [Fichero] varbinary(max) NULL,
    [FicheroUrl] nvarchar(max) NULL,
    [FicheroNombre] nvarchar(max) NULL,
    CONSTRAINT [PK_CompraFacturas] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CompraFacturaLineas] (
    [IdFacturaLinea] int NOT NULL IDENTITY,
    [CompraFacturaId] int NOT NULL,
    [CodigoArticulo] nvarchar(max) NOT NULL,
    [NombreArticulo] nvarchar(max) NULL,
    [ObservacionesFacturaLinea] nvarchar(max) NULL,
    [CodigoEvento] nvarchar(max) NOT NULL,
    [NombreEvento] nvarchar(max) NULL,
    [Unidades] decimal(18,2) NOT NULL,
    [BaseImponiblePrecioUnitario] decimal(18,2) NOT NULL,
    [BaseImponibleTotal] decimal(18,2) NOT NULL,
    [IVA_Porcentaje] int NOT NULL,
    [TieneRetencion] bit NOT NULL,
    [Retencion_Porcentaje] int NULL,
    [Orden] int NOT NULL,
    CONSTRAINT [PK_CompraFacturaLineas] PRIMARY KEY ([IdFacturaLinea]),
    CONSTRAINT [FK_CompraFacturaLineas_CompraFacturas_CompraFacturaId] FOREIGN KEY ([CompraFacturaId]) REFERENCES [CompraFacturas] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_CompraAlbaranes_CompraFacturaId] ON [CompraAlbaranes] ([CompraFacturaId]);

GO

CREATE INDEX [IX_CompraFacturaLineas_CompraFacturaId] ON [CompraFacturaLineas] ([CompraFacturaId]);

GO

ALTER TABLE [CompraAlbaranes] ADD CONSTRAINT [FK_CompraAlbaranes_CompraFacturas_CompraFacturaId] FOREIGN KEY ([CompraFacturaId]) REFERENCES [CompraFacturas] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210111175630_CompraFacturas', N'3.1.2');

GO


DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturaLineas]') AND [c].[name] = N'Retencion_Porcentaje');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturaLineas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CompraFacturaLineas] DROP COLUMN [Retencion_Porcentaje];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturaLineas]') AND [c].[name] = N'TieneRetencion');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturaLineas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [CompraFacturaLineas] DROP COLUMN [TieneRetencion];

GO

ALTER TABLE [CompraFacturas] ADD [Retencion_Porcentaje] int NULL;

GO

ALTER TABLE [CompraFacturas] ADD [TieneRetencion] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210112161833_retencionBien', N'3.1.2');

GO



ALTER TABLE [CompraFacturaLineas] ADD [CompraAlbaranLineaId] int NULL;

GO

CREATE INDEX [IX_CompraFacturaLineas_CompraAlbaranLineaId] ON [CompraFacturaLineas] ([CompraAlbaranLineaId]);

GO

ALTER TABLE [CompraFacturaLineas] ADD CONSTRAINT [FK_CompraFacturaLineas_CompraAlbaranLineas_CompraAlbaranLineaId] FOREIGN KEY ([CompraAlbaranLineaId]) REFERENCES [CompraAlbaranLineas] ([IdAlbaranLinea]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210113105838_cambioDesconocido', N'3.1.2');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturas]') AND [c].[name] = N'Fichero');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CompraFacturas] DROP COLUMN [Fichero];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CompraFacturas]') AND [c].[name] = N'TieneFichero');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CompraFacturas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [CompraFacturas] DROP COLUMN [TieneFichero];

GO


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210115163825_refactorizarFacturaCompra', N'3.1.2');

GO

ALTER TABLE [CompraFacturaLineas] ADD [CompraPedidoLineaId] int NULL;

GO

CREATE INDEX [IX_CompraFacturaLineas_CompraPedidoLineaId] ON [CompraFacturaLineas] ([CompraPedidoLineaId]);

GO

ALTER TABLE [CompraFacturaLineas] ADD CONSTRAINT [FK_CompraFacturaLineas_CompraPedidoLineas_CompraPedidoLineaId] FOREIGN KEY ([CompraPedidoLineaId]) REFERENCES [CompraPedidoLineas] ([IdPedidoLinea]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210118093003_compraPedidoLineaId', N'3.1.2');

GO



