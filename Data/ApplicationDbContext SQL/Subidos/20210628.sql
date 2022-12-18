ALTER TABLE [Personal] ADD [CT] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Personal] ADD [FechaApto] datetime2 NULL;

GO

ALTER TABLE [Personal] ADD [FechaValidezNIF] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

GO

ALTER TABLE [Personal] ADD [RevisionMedica] nvarchar(max) NULL;

GO

ALTER TABLE [Personal] ADD [SAP] nvarchar(3) NULL;

GO

ALTER TABLE [Personal] ADD [Venta] decimal(13,2) NULL;

GO

ALTER TABLE [GrabacionE9] ADD [Descripcion] nvarchar(80) NOT NULL DEFAULT N'';

GO

CREATE TABLE [Documento] (
    [Id] int NOT NULL IDENTITY,
    [FicheroUrl] nvarchar(max) NULL,
    [FicheroNombre] nvarchar(max) NULL,
    [Tipo] int NOT NULL,
    [PersonalId] int NOT NULL,
    CONSTRAINT [PK_Documento] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Documento_Personal_PersonalId] FOREIGN KEY ([PersonalId]) REFERENCES [Personal] ([Id]) ON DELETE CASCADE
);

GO


CREATE INDEX [IX_Documento_PersonalId] ON [Documento] ([PersonalId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210628071721_documentos', N'3.1.2');

GO

