IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Clients] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [SageCode] nvarchar(max) NULL,
    [BusinessName] nvarchar(max) NOT NULL,
    [NIF] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Email2] nvarchar(max) NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Congresses] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Number] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [Place] nvarchar(max) NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [LogoBase64] nvarchar(max) NULL,
    [TailBase64] nvarchar(max) NULL,
    [Code] nvarchar(5) NOT NULL,
    [ConnectionString] nvarchar(max) NULL,
    [HideRegistrations] bit NOT NULL,
    [CertificateNumber] nvarchar(max) NULL,
    [CertificateCreditor] nvarchar(max) NULL,
    [CertificateCredits] float NOT NULL,
    [CertificateCity] nvarchar(max) NULL,
    [CertificateTime] datetime2 NOT NULL,
    CONSTRAINT [PK_Congresses] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RegistrantLocations] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Address] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [ZipCode] nvarchar(max) NOT NULL,
    [Province] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [Phone2] nvarchar(max) NULL,
    [RegistrantId] uniqueidentifier NULL,
    CONSTRAINT [PK_RegistrantLocations] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RegistrationTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RegistrationTypes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RoomTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RoomTypes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Treatments] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Treatments] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ClientLocations] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Address] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [ZipCode] nvarchar(max) NOT NULL,
    [Province] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [Phone2] nvarchar(max) NULL,
    [SageLine] int NULL,
    [SageClient] nvarchar(max) NULL,
    [ClientId] uniqueidentifier NULL,
    CONSTRAINT [PK_ClientLocations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientLocations_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Accounts] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Name] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [CongressId] uniqueidentifier NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Accounts_Congresses_CongressId] FOREIGN KEY ([CongressId]) REFERENCES [Congresses] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Registrant] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Name] nvarchar(max) NOT NULL,
    [Surnames] nvarchar(max) NOT NULL,
    [TreatmentId] uniqueidentifier NOT NULL,
    [Position] nvarchar(max) NULL,
    [ProfessionalCategory] nvarchar(max) NULL,
    [Workplace] nvarchar(max) NULL,
    [LocationId] uniqueidentifier NOT NULL,
    [NIF] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Email2] nvarchar(max) NULL,
    CONSTRAINT [PK_Registrant] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Registrant_RegistrantLocations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [RegistrantLocations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Registrant_Treatments_TreatmentId] FOREIGN KEY ([TreatmentId]) REFERENCES [Treatments] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Accommodations] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [CongressId] uniqueidentifier NOT NULL,
    [RegistrantId] uniqueidentifier NOT NULL,
    [ClientId] uniqueidentifier NULL,
    [Fee] decimal(18,2) NOT NULL,
    [VATId] nvarchar(max) NOT NULL,
    [BillingLocationId] uniqueidentifier NULL,
    [Notified] bit NOT NULL,
    [Authorization] bit NOT NULL,
    [OnlyBilling] bit NOT NULL,
    [Exported] bit NOT NULL,
    [Imported] bit NOT NULL,
    [Paid] bit NOT NULL,
    [Reviewed] bit NOT NULL,
    [Notes] nvarchar(max) NULL,
    [CompanionId] uniqueidentifier NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Hotel] nvarchar(max) NOT NULL,
    [RoomTypeId] uniqueidentifier NOT NULL,
    [Occupants] int NOT NULL,
    CONSTRAINT [PK_Accommodations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Accommodations_ClientLocations_BillingLocationId] FOREIGN KEY ([BillingLocationId]) REFERENCES [ClientLocations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Accommodations_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Accommodations_Registrant_CompanionId] FOREIGN KEY ([CompanionId]) REFERENCES [Registrant] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Accommodations_Congresses_CongressId] FOREIGN KEY ([CongressId]) REFERENCES [Congresses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Accommodations_Registrant_RegistrantId] FOREIGN KEY ([RegistrantId]) REFERENCES [Registrant] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Accommodations_RoomTypes_RoomTypeId] FOREIGN KEY ([RoomTypeId]) REFERENCES [RoomTypes] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Registrations] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [CongressId] uniqueidentifier NOT NULL,
    [RegistrantId] uniqueidentifier NOT NULL,
    [ClientId] uniqueidentifier NULL,
    [Fee] decimal(18,2) NOT NULL,
    [VATId] nvarchar(max) NOT NULL,
    [BillingLocationId] uniqueidentifier NULL,
    [Notified] bit NOT NULL,
    [Authorization] bit NOT NULL,
    [OnlyBilling] bit NOT NULL,
    [Exported] bit NOT NULL,
    [Imported] bit NOT NULL,
    [Paid] bit NOT NULL,
    [Reviewed] bit NOT NULL,
    [Notes] nvarchar(max) NULL,
    [Number] int NOT NULL,
    [RegistrationTypeId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Registrations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Registrations_ClientLocations_BillingLocationId] FOREIGN KEY ([BillingLocationId]) REFERENCES [ClientLocations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Registrations_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Registrations_Congresses_CongressId] FOREIGN KEY ([CongressId]) REFERENCES [Congresses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Registrations_Registrant_RegistrantId] FOREIGN KEY ([RegistrantId]) REFERENCES [Registrant] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Registrations_RegistrationTypes_RegistrationTypeId] FOREIGN KEY ([RegistrationTypeId]) REFERENCES [RegistrationTypes] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Accommodations_BillingLocationId] ON [Accommodations] ([BillingLocationId]);

GO

CREATE INDEX [IX_Accommodations_ClientId] ON [Accommodations] ([ClientId]);

GO

CREATE INDEX [IX_Accommodations_CompanionId] ON [Accommodations] ([CompanionId]);

GO

CREATE INDEX [IX_Accommodations_CongressId] ON [Accommodations] ([CongressId]);

GO

CREATE INDEX [IX_Accommodations_RegistrantId] ON [Accommodations] ([RegistrantId]);

GO

CREATE INDEX [IX_Accommodations_RoomTypeId] ON [Accommodations] ([RoomTypeId]);

GO

CREATE INDEX [IX_Accounts_CongressId] ON [Accounts] ([CongressId]);

GO

CREATE INDEX [IX_ClientLocations_ClientId] ON [ClientLocations] ([ClientId]);

GO

CREATE UNIQUE INDEX [IX_Registrant_LocationId] ON [Registrant] ([LocationId]);

GO

CREATE INDEX [IX_Registrant_TreatmentId] ON [Registrant] ([TreatmentId]);

GO

CREATE INDEX [IX_Registrations_BillingLocationId] ON [Registrations] ([BillingLocationId]);

GO

CREATE INDEX [IX_Registrations_ClientId] ON [Registrations] ([ClientId]);

GO

CREATE INDEX [IX_Registrations_CongressId] ON [Registrations] ([CongressId]);

GO

CREATE INDEX [IX_Registrations_RegistrantId] ON [Registrations] ([RegistrantId]);

GO

CREATE INDEX [IX_Registrations_RegistrationTypeId] ON [Registrations] ([RegistrationTypeId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200107085706_Recreate', N'3.1.2');

GO

ALTER TABLE [Congresses] ADD [DatabasePrefix] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200107151537_DbPrefix', N'3.1.2');

GO

ALTER TABLE [Registrant] ADD [Category] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200108112159_RegistrantCategory', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [InvoiceNumber] nvarchar(max) NULL;

GO

ALTER TABLE [Accommodations] ADD [InvoiceNumber] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200115104052_InvoiceNumber', N'3.1.2');

GO

ALTER TABLE [Congresses] ADD [Ended] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200116075851_CongressEnded', N'3.1.2');

GO

ALTER TABLE [Congresses] ADD [Attendants] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Congresses] ADD [Budget] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [Congresses] ADD [Catering] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [CommunicationsAndSocialMedia] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [CommunicationsAndType] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [CorporatePhone] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Email] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Email2] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Financing] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Headquarters] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Infrastructures] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [InscriptionFee] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [Congresses] ADD [InternalCode] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Congresses] ADD [MailPassword] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [MailPort] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Congresses] ADD [MailServer] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [MailUser] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Notes] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Organizer] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [PersonalPhone] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Position] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [President] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [ScientificSecretariat] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [SpeakersHotel] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [TechnicalSecretariat] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Travels] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [WebDomain] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Workplace] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200117125951_ExtraCongressData', N'3.1.2');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Email');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Clients] ALTER COLUMN [Email] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200120115852_ClientEmailOptional', N'3.1.2');

GO

ALTER TABLE [Accommodations] ADD [Number] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200124105834_MoveAccomNumber', N'3.1.2');

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accommodations]') AND [c].[name] = N'Occupants');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Accommodations] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Accommodations] DROP COLUMN [Occupants];

GO

ALTER TABLE [RoomTypes] ADD [Occupants] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200129100159_RoomOccupants', N'3.1.2');

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Congresses]') AND [c].[name] = N'MailPort');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Congresses] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Congresses] DROP COLUMN [MailPort];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Congresses]') AND [c].[name] = N'MailServer');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Congresses] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Congresses] DROP COLUMN [MailServer];

GO

ALTER TABLE [Congresses] ADD [IBAN] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [IncomingMailPort] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Congresses] ADD [IncomingMailServer] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [OutgoingMailPort] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Congresses] ADD [OutgoingMailServer] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [SageAccount] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [Signature] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200224100433_ExtraCongressFields', N'3.1.2');

GO

ALTER TABLE [Congresses] ADD [SignatureAfter] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [SignatureBefore] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200227121647_SignatureExtra', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [PaidDate] datetime2 NULL;

GO

ALTER TABLE [Accommodations] ADD [PaidDate] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200303144740_PaidDate', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [InvoiceDate] datetime2 NULL;

GO

ALTER TABLE [Accommodations] ADD [InvoiceDate] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200312085553_InvoiceDate', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [AccommodationId] uniqueidentifier NULL;

GO

ALTER TABLE [Accommodations] ADD [RegistrationId] uniqueidentifier NULL;

GO

CREATE UNIQUE INDEX [IX_Accommodations_RegistrationId] ON [Accommodations] ([RegistrationId]) WHERE [RegistrationId] IS NOT NULL;

GO

ALTER TABLE [Accommodations] ADD CONSTRAINT [FK_Accommodations_Registrations_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [Registrations] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200324141916_RegistrationsLink', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [AccountId] uniqueidentifier NULL;

GO

ALTER TABLE [Registrations] ADD [Product] nvarchar(max) NULL DEFAULT N'70100';

GO

ALTER TABLE [Registrations] ADD [Serie] nvarchar(max) NULL DEFAULT N'I ';

GO

ALTER TABLE [Registrations] ADD [Units] float NOT NULL DEFAULT 1.0E0;

GO

ALTER TABLE [Congresses] ADD [InvoiceDataId] uniqueidentifier NULL;

GO

ALTER TABLE [Congresses] ADD [Warehouse] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Accounts] ADD [Vendedor] nvarchar(max) NULL;

GO

ALTER TABLE [Accommodations] ADD [AccountId] uniqueidentifier NULL;

GO

ALTER TABLE [Accommodations] ADD [Product] nvarchar(max) NULL DEFAULT N'70800';

GO

ALTER TABLE [Accommodations] ADD [Serie] nvarchar(max) NULL DEFAULT N'A ';

GO

ALTER TABLE [Accommodations] ADD [Units] float NOT NULL DEFAULT 1.0E0;

GO

CREATE TABLE [Expenses] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [CongressId] uniqueidentifier NOT NULL,
    [ClientId] uniqueidentifier NULL,
    [BillingLocationId] uniqueidentifier NULL,
    [AccountId] uniqueidentifier NULL,
    [Product] nvarchar(max) NULL,
    [Serie] nvarchar(max) NULL,
    [Fee] decimal(18,2) NOT NULL,
    [Units] float NOT NULL,
    [VATId] nvarchar(max) NOT NULL,
    [Number] int NOT NULL,
    [InvoiceNumber] nvarchar(max) NULL,
    [InvoiceDate] datetime2 NULL,
    [PaidDate] datetime2 NULL,
    [Imported] bit NOT NULL,
    [Paid] bit NOT NULL,
    [Notified] bit NOT NULL,
    [Authorization] bit NOT NULL,
    [OnlyBilling] bit NOT NULL,
    [Exported] bit NOT NULL,
    [Reviewed] bit NOT NULL,
    [Notes] nvarchar(max) NULL,
    CONSTRAINT [PK_Expenses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Expenses_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Expenses_ClientLocations_BillingLocationId] FOREIGN KEY ([BillingLocationId]) REFERENCES [ClientLocations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Expenses_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Expenses_Congresses_CongressId] FOREIGN KEY ([CongressId]) REFERENCES [Congresses] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [InvoiceData] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [LogoBase64] nvarchar(max) NULL,
    [TailBase64] nvarchar(max) NULL,
    [IBAN] nvarchar(max) NULL,
    [SageAccount] nvarchar(max) NULL,
    CONSTRAINT [PK_InvoiceData] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_Registrations_AccountId] ON [Registrations] ([AccountId]);

GO

CREATE INDEX [IX_Congresses_InvoiceDataId] ON [Congresses] ([InvoiceDataId]);

GO

CREATE INDEX [IX_Accommodations_AccountId] ON [Accommodations] ([AccountId]);

GO

CREATE INDEX [IX_Expenses_AccountId] ON [Expenses] ([AccountId]);

GO

CREATE INDEX [IX_Expenses_BillingLocationId] ON [Expenses] ([BillingLocationId]);

GO

CREATE INDEX [IX_Expenses_ClientId] ON [Expenses] ([ClientId]);

GO

CREATE INDEX [IX_Expenses_CongressId] ON [Expenses] ([CongressId]);

GO

ALTER TABLE [Accommodations] ADD CONSTRAINT [FK_Accommodations_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Congresses] ADD CONSTRAINT [FK_Congresses_InvoiceData_InvoiceDataId] FOREIGN KEY ([InvoiceDataId]) REFERENCES [InvoiceData] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Registrations] ADD CONSTRAINT [FK_Registrations_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200423105006_AddCommonData', N'3.1.2');

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Congresses]') AND [c].[name] = N'Warehouse');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Congresses] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Congresses] DROP COLUMN [Warehouse];

GO

CREATE TABLE [Product] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [ExpenseId] uniqueidentifier NOT NULL,
    [ProductCode] nvarchar(max) NULL,
    [Serie] nvarchar(max) NULL,
    [Fee] decimal(18,2) NOT NULL,
    [Units] float NOT NULL,
    [VATId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Product_Expenses_ExpenseId] FOREIGN KEY ([ExpenseId]) REFERENCES [Expenses] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Product_ExpenseId] ON [Product] ([ExpenseId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200424082411_AddMultipleProducts', N'3.1.2');

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Product]') AND [c].[name] = N'Serie');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Product] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Product] DROP COLUMN [Serie];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200424102901_ProductRemoveSerie', N'3.1.2');

GO

ALTER TABLE [Product] DROP CONSTRAINT [FK_Product_Expenses_ExpenseId];

GO

ALTER TABLE [Product] DROP CONSTRAINT [PK_Product];

GO

EXEC sp_rename N'[Product]', N'Products';

GO

EXEC sp_rename N'[Products].[IX_Product_ExpenseId]', N'IX_Products_ExpenseId', N'INDEX';

GO

ALTER TABLE [Products] ADD CONSTRAINT [PK_Products] PRIMARY KEY ([Id]);

GO

ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_Expenses_ExpenseId] FOREIGN KEY ([ExpenseId]) REFERENCES [Expenses] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200424104428_AddProductsToContext', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [ProductDescription] nvarchar(max) NULL;

GO

ALTER TABLE [Registrations] ADD [VAT] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [Products] ADD [ProductDescription] nvarchar(max) NULL;

GO

ALTER TABLE [Products] ADD [VAT] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [Expenses] ADD [ProductDescription] nvarchar(max) NULL;

GO

ALTER TABLE [Expenses] ADD [VAT] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [Accommodations] ADD [ProductDescription] nvarchar(max) NULL;

GO

ALTER TABLE [Accommodations] ADD [VAT] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200427120314_ProductDescription', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [ProductNotes] nvarchar(max) NULL;

GO

ALTER TABLE [Products] ADD [ProductNotes] nvarchar(max) NULL;

GO

ALTER TABLE [Expenses] ADD [ProductNotes] nvarchar(max) NULL;

GO

ALTER TABLE [Accommodations] ADD [ProductNotes] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200427123900_ProductNotes', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [FPag] nvarchar(max) NOT NULL DEFAULT N'';

GO

ALTER TABLE [Expenses] ADD [FPag] nvarchar(max) NOT NULL DEFAULT N'';

GO

ALTER TABLE [Accommodations] ADD [FPag] nvarchar(max) NOT NULL DEFAULT N'';

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200428134316_FormaDePargo', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [DocumentTypeId] uniqueidentifier NOT NULL DEFAULT '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';

GO

ALTER TABLE [Expenses] ADD [DocumentTypeId] uniqueidentifier NOT NULL DEFAULT '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';

GO

ALTER TABLE [Accommodations] ADD [DocumentTypeId] uniqueidentifier NOT NULL DEFAULT '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';

GO

CREATE TABLE [DocumentTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_DocumentTypes] PRIMARY KEY ([Id])
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Deleted', N'Modified', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] ON;
INSERT INTO [DocumentTypes] ([Id], [Created], [Deleted], [Modified], [Name])
VALUES ('51e91cdd-bfbd-4081-8a60-eaee0a9bea39', '2020-04-28T16:06:33.0330190+02:00', NULL, '2020-04-28T16:06:33.0330762+02:00', N'Factura');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Deleted', N'Modified', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Deleted', N'Modified', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] ON;
INSERT INTO [DocumentTypes] ([Id], [Created], [Deleted], [Modified], [Name])
VALUES ('043d73bf-1516-4672-affe-4f0836048f40', '2020-04-28T16:06:33.0331356+02:00', NULL, '2020-04-28T16:06:33.0331367+02:00', N'Factura proforma');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Deleted', N'Modified', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Deleted', N'Modified', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] ON;
INSERT INTO [DocumentTypes] ([Id], [Created], [Deleted], [Modified], [Name])
VALUES ('fd4d4c0d-53cb-4844-a520-cba0dac861c4', '2020-04-28T16:06:33.0331387+02:00', NULL, '2020-04-28T16:06:33.0331390+02:00', N'Presupuesto');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Deleted', N'Modified', N'Name') AND [object_id] = OBJECT_ID(N'[DocumentTypes]'))
    SET IDENTITY_INSERT [DocumentTypes] OFF;

GO

CREATE INDEX [IX_Registrations_DocumentTypeId] ON [Registrations] ([DocumentTypeId]);

GO

CREATE INDEX [IX_Expenses_DocumentTypeId] ON [Expenses] ([DocumentTypeId]);

GO

CREATE INDEX [IX_Accommodations_DocumentTypeId] ON [Accommodations] ([DocumentTypeId]);

GO

ALTER TABLE [Accommodations] ADD CONSTRAINT [FK_Accommodations_DocumentTypes_DocumentTypeId] FOREIGN KEY ([DocumentTypeId]) REFERENCES [DocumentTypes] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Expenses] ADD CONSTRAINT [FK_Expenses_DocumentTypes_DocumentTypeId] FOREIGN KEY ([DocumentTypeId]) REFERENCES [DocumentTypes] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Registrations] ADD CONSTRAINT [FK_Registrations_DocumentTypes_DocumentTypeId] FOREIGN KEY ([DocumentTypeId]) REFERENCES [DocumentTypes] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200428140633_DocumentType', N'3.1.2');

GO

ALTER TABLE [Accounts] ADD [AccessAdmin] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Accounts] ADD [AccessBudgetControl] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Accounts] ADD [AccessCollaborations] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Accounts] ADD [AccessCongress] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-28T16:47:15.5049901+02:00', [Modified] = '2020-04-28T16:47:15.5049911+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-28T16:47:15.5048761+02:00', [Modified] = '2020-04-28T16:47:15.5049322+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-28T16:47:15.5049932+02:00', [Modified] = '2020-04-28T16:47:15.5049935+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200428144716_AccountRoles', N'3.1.2');

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Registrations]') AND [c].[name] = N'Serie');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Registrations] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Registrations] ALTER COLUMN [Serie] nvarchar(max) NULL;
ALTER TABLE [Registrations] ADD DEFAULT N'I ' FOR [Serie];

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Registrations]') AND [c].[name] = N'Product');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Registrations] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Registrations] ALTER COLUMN [Product] nvarchar(max) NULL;
ALTER TABLE [Registrations] ADD DEFAULT N'70100' FOR [Product];

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accommodations]') AND [c].[name] = N'Serie');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Accommodations] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Accommodations] ALTER COLUMN [Serie] nvarchar(max) NULL;
ALTER TABLE [Accommodations] ADD DEFAULT N'A ' FOR [Serie];

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accommodations]') AND [c].[name] = N'Product');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Accommodations] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Accommodations] ALTER COLUMN [Product] nvarchar(max) NULL;
ALTER TABLE [Accommodations] ADD DEFAULT N'70800' FOR [Product];

GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-29T16:02:09.8701654+02:00', [Modified] = '2020-04-29T16:02:09.8701668+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-29T16:02:09.8700551+02:00', [Modified] = '2020-04-29T16:02:09.8701094+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-29T16:02:09.8701688+02:00', [Modified] = '2020-04-29T16:02:09.8701691+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200429140210_DefaultValues', N'3.1.2');

GO

CREATE TABLE [CongressEmailAccounts] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Modified] datetime2 NOT NULL,
    [Deleted] datetime2 NULL,
    [CongressId] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [IncomingMailServer] nvarchar(max) NULL,
    [OutgoingMailServer] nvarchar(max) NULL,
    [IncomingMailPort] int NOT NULL,
    [OutgoingMailPort] int NOT NULL,
    [MailUser] nvarchar(max) NULL,
    [MailPassword] nvarchar(max) NULL,
    [SignatureBefore] nvarchar(max) NULL,
    [Signature] nvarchar(max) NULL,
    [SignatureAfter] nvarchar(max) NULL,
    CONSTRAINT [PK_CongressEmailAccounts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CongressEmailAccounts_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CongressEmailAccounts_Congresses_CongressId] FOREIGN KEY ([CongressId]) REFERENCES [Congresses] ([Id]) ON DELETE CASCADE
);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-30T17:29:18.9888392+02:00', [Modified] = '2020-04-30T17:29:18.9888403+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-30T17:29:18.9887264+02:00', [Modified] = '2020-04-30T17:29:18.9887818+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-04-30T17:29:18.9888424+02:00', [Modified] = '2020-04-30T17:29:18.9888427+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

CREATE INDEX [IX_CongressEmailAccounts_AccountId] ON [CongressEmailAccounts] ([AccountId]);

GO

CREATE UNIQUE INDEX [IX_CongressEmailAccounts_CongressId_AccountId] ON [CongressEmailAccounts] ([CongressId], [AccountId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200430152919_CongressEmails', N'3.1.2');

GO

ALTER TABLE [Registrations] ADD [ShowCostCenterInfoOnInvoice] bit NOT NULL DEFAULT CAST(1 AS bit);

GO

ALTER TABLE [Expenses] ADD [ShowCostCenterInfoOnInvoice] bit NOT NULL DEFAULT CAST(1 AS bit);

GO

ALTER TABLE [Accommodations] ADD [ShowCostCenterInfoOnInvoice] bit NOT NULL DEFAULT CAST(1 AS bit);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T12:16:03.0511647+02:00', [Modified] = '2020-05-04T12:16:03.0511657+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T12:16:03.0510530+02:00', [Modified] = '2020-05-04T12:16:03.0511068+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T12:16:03.0511677+02:00', [Modified] = '2020-05-04T12:16:03.0511681+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200504101603_ShowDataOnInvoice', N'3.1.2');

GO

ALTER TABLE [Congresses] ADD [IsCongress] bit NOT NULL DEFAULT CAST(1 AS bit);

GO

ALTER TABLE [Accounts] ADD [AccessMaintenece] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T15:16:19.3146896+02:00', [Modified] = '2020-05-04T15:16:19.3146907+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T15:16:19.3145691+02:00', [Modified] = '2020-05-04T15:16:19.3146276+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T15:16:19.3146931+02:00', [Modified] = '2020-05-04T15:16:19.3146934+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200504131619_IsCongressAndMaintenance', N'3.1.2');

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'AccessMaintenece');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Accounts] DROP COLUMN [AccessMaintenece];

GO

ALTER TABLE [Accounts] ADD [AccessMaintenance] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T15:33:06.5358602+02:00', [Modified] = '2020-05-04T15:33:06.5358613+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T15:33:06.5357419+02:00', [Modified] = '2020-05-04T15:33:06.5357998+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-04T15:33:06.5358635+02:00', [Modified] = '2020-05-04T15:33:06.5358638+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200504133307_TypoMaintenance', N'3.1.2');

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Congresses]') AND [c].[name] = N'IsCongress');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Congresses] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Congresses] ALTER COLUMN [IsCongress] bit NOT NULL;

GO

ALTER TABLE [Accounts] ADD [Email] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [SendCopyTo] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-15T08:54:10.9149068+02:00', [Modified] = '2020-05-15T08:54:10.9149079+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-15T08:54:10.9147937+02:00', [Modified] = '2020-05-15T08:54:10.9148490+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-15T08:54:10.9149101+02:00', [Modified] = '2020-05-15T08:54:10.9149104+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200515065411_SendMailCopy', N'3.1.2');

GO

ALTER TABLE [Accounts] ADD [IncomingMailPort] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Accounts] ADD [IncomingMailServer] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [MailPassword] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [MailUser] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [OutgoingMailPort] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Accounts] ADD [OutgoingMailServer] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [Signature] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [SignatureAfter] nvarchar(max) NULL;

GO

ALTER TABLE [Accounts] ADD [SignatureBefore] nvarchar(max) NULL;

GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-26T13:49:38.2936845+02:00', [Modified] = '2020-05-26T13:49:38.2936859+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-26T13:49:38.2935563+02:00', [Modified] = '2020-05-26T13:49:38.2936140+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-05-26T13:49:38.2936882+02:00', [Modified] = '2020-05-26T13:49:38.2936885+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200526114938_AccoundDefaultEmail', N'3.1.2');

GO

ALTER TABLE [Congresses] ADD [Database] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [DatabasePassword] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [DatabaseServer] nvarchar(max) NULL;

GO

ALTER TABLE [Congresses] ADD [DatabaseUser] nvarchar(max) NULL;

GO

UPDATE [DocumentTypes] SET [Created] = '2020-06-09T11:45:02.6866572+02:00', [Modified] = '2020-06-09T11:45:02.6866581+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-06-09T11:45:02.6865478+02:00', [Modified] = '2020-06-09T11:45:02.6866011+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-06-09T11:45:02.6866602+02:00', [Modified] = '2020-06-09T11:45:02.6866606+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200609094503_databaseFields', N'3.1.2');

GO

ALTER TABLE [Accounts] ADD [AccessInvoices] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

UPDATE [DocumentTypes] SET [Created] = '2020-09-22T08:21:14.7123773+02:00', [Modified] = '2020-09-22T08:21:14.7123785+02:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-09-22T08:21:14.7122297+02:00', [Modified] = '2020-09-22T08:21:14.7122875+02:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-09-22T08:21:14.7123804+02:00', [Modified] = '2020-09-22T08:21:14.7123807+02:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200922062115_AccessInvoices', N'3.1.2');

GO



UPDATE [DocumentTypes] SET [Created] = '2020-12-26T13:57:24.9292719+01:00', [Modified] = '2020-12-26T13:57:24.9292729+01:00'
WHERE [Id] = '043d73bf-1516-4672-affe-4f0836048f40';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-12-26T13:57:24.9291613+01:00', [Modified] = '2020-12-26T13:57:24.9292150+01:00'
WHERE [Id] = '51e91cdd-bfbd-4081-8a60-eaee0a9bea39';
SELECT @@ROWCOUNT;


GO

UPDATE [DocumentTypes] SET [Created] = '2020-12-26T13:57:24.9292750+01:00', [Modified] = '2020-12-26T13:57:24.9292753+01:00'
WHERE [Id] = 'fd4d4c0d-53cb-4844-a520-cba0dac861c4';
SELECT @@ROWCOUNT;


GO

CREATE INDEX [IX_CompraPedidoLineas_PedidoId] ON [CompraPedidoLineas] ([PedidoId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201226125725_pedidocompras', N'3.1.2');

GO

