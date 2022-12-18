ALTER TABLE [CompraFacturas] ADD [Pagada] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210407094437_pagada', N'3.1.2');

GO

