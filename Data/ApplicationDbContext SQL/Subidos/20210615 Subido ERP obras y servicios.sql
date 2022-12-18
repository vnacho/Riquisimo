CREATE TABLE [TiposCentroCoste] (
    [Id] int NOT NULL IDENTITY,
    [Tipo] nvarchar(1) NOT NULL,
    [Descripcion] nvarchar(80) NOT NULL,
    CONSTRAINT [PK_TiposCentroCoste] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210615102710_tiposcentrocoste', N'3.1.2');

GO

CREATE TABLE [CentrosCoste] (
    [Id] int NOT NULL IDENTITY,
    [NivelAnalitico1] nvarchar(3) NOT NULL,
    [NivelAnalitico2] nvarchar(5) NOT NULL,
    [Nombre] nvarchar(80) NOT NULL,
    [TipoCentroCosteId] int NOT NULL,
    CONSTRAINT [PK_CentrosCoste] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CentrosCoste_TiposCentroCoste_TipoCentroCosteId] FOREIGN KEY ([TipoCentroCosteId]) REFERENCES [TiposCentroCoste] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_CentrosCoste_TipoCentroCosteId] ON [CentrosCoste] ([TipoCentroCosteId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210615160529_centrocoste', N'3.1.2');

GO

ALTER TABLE [TiposCentroCoste] ADD [PorcentajeDistribucion] decimal(3,2) NOT NULL DEFAULT 0.0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210615163544_porcentajedistribucion', N'3.1.2');

GO


DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TiposCentroCoste]') AND [c].[name] = N'PorcentajeDistribucion');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [TiposCentroCoste] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [TiposCentroCoste] ALTER COLUMN [PorcentajeDistribucion] decimal(5,2) NOT NULL;

GO


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210615170113_porcentajedistribucion52', N'3.1.2');

GO

CREATE TABLE [Estructura] (
    [Id] int NOT NULL IDENTITY,
    [PorcentajeReparto] decimal(5,2) NOT NULL,
    [CentroCosteId] int NOT NULL,
    CONSTRAINT [PK_Estructura] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Estructura_CentrosCoste_CentroCosteId] FOREIGN KEY ([CentroCosteId]) REFERENCES [CentrosCoste] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [GrabacionE9] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [EntradaSalida] nvarchar(1) NOT NULL,
    [Importe] decimal(18,2) NOT NULL,
    [CentroCosteId] int NOT NULL,
    CONSTRAINT [PK_GrabacionE9] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GrabacionE9_CentrosCoste_CentroCosteId] FOREIGN KEY ([CentroCosteId]) REFERENCES [CentrosCoste] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TiposTarifa] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] nvarchar(1) NOT NULL,
    [Nombre] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_TiposTarifa] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Personal] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [NIF] nvarchar(max) NOT NULL,
    [Categoria] nvarchar(40) NOT NULL,
    [CosteEstandar] decimal(13,2) NULL,
    [FechaAlta] datetime2 NULL,
    [FechaUltimaRevisionMedica] datetime2 NULL,
    [FechaBaja] datetime2 NULL,
    [IBAN] nvarchar(max) NULL,
    [TipoTarifaId] int NOT NULL,
    [CentroCosteId] int NOT NULL,
    CONSTRAINT [PK_Personal] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Personal_CentrosCoste_CentroCosteId] FOREIGN KEY ([CentroCosteId]) REFERENCES [CentrosCoste] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Personal_TiposTarifa_TipoTarifaId] FOREIGN KEY ([TipoTarifaId]) REFERENCES [TiposTarifa] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [PartePersonal] (
    [Id] int NOT NULL IDENTITY,
    [TipoParte] nvarchar(1) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Unidades] decimal(18,2) NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Importe] decimal(18,2) NOT NULL,
    [PersonalId] int NOT NULL,
    [CentroCosteId] int NOT NULL,
    [PersonalId1] int NULL,
    CONSTRAINT [PK_PartePersonal] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PartePersonal_CentrosCoste_CentroCosteId] FOREIGN KEY ([CentroCosteId]) REFERENCES [CentrosCoste] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PartePersonal_Personal_PersonalId] FOREIGN KEY ([PersonalId]) REFERENCES [Personal] ([Id]),
    CONSTRAINT [FK_PartePersonal_Personal_PersonalId1] FOREIGN KEY ([PersonalId1]) REFERENCES [Personal] ([Id]) ON DELETE NO ACTION
);

GO
CREATE INDEX [IX_Estructura_CentroCosteId] ON [Estructura] ([CentroCosteId]);

GO

CREATE INDEX [IX_GrabacionE9_CentroCosteId] ON [GrabacionE9] ([CentroCosteId]);

GO

CREATE INDEX [IX_PartePersonal_CentroCosteId] ON [PartePersonal] ([CentroCosteId]);

GO

CREATE INDEX [IX_PartePersonal_PersonalId] ON [PartePersonal] ([PersonalId]);

GO

CREATE INDEX [IX_PartePersonal_PersonalId1] ON [PartePersonal] ([PersonalId1]);

GO

CREATE INDEX [IX_Personal_CentroCosteId] ON [Personal] ([CentroCosteId]);

GO

CREATE INDEX [IX_Personal_TipoTarifaId] ON [Personal] ([TipoTarifaId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210618130942_erpobras', N'3.1.2');

GO

ALTER TABLE [GrabacionE9] ADD [DestinoId] int NOT NULL DEFAULT 0;

GO

CREATE INDEX [IX_GrabacionE9_DestinoId] ON [GrabacionE9] ([DestinoId]);

GO

ALTER TABLE [GrabacionE9] ADD CONSTRAINT [FK_GrabacionE9_CentrosCoste_DestinoId] FOREIGN KEY ([DestinoId]) REFERENCES [CentrosCoste] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210618150528_grabaciondestino', N'3.1.2');

GO






