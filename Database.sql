IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Banks] (
    [Id] int NOT NULL IDENTITY,
    [BankName] nvarchar(20) NOT NULL,
    [BankAddress] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Banks] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [CustomerName] nvarchar(20) NOT NULL,
    [CustomerAddress] nvarchar(100) NOT NULL,
    [BankId] int NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Customers_Banks_BankId] FOREIGN KEY ([BankId]) REFERENCES [Banks] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Accounts] (
    [Id] int NOT NULL IDENTITY,
    [Balance] decimal(8,2) NOT NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Accounts_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Transactions] (
    [Id] int NOT NULL IDENTITY,
    [Money] decimal(8,2) NOT NULL,
    [TransactionDate] date NOT NULL,
    [AccountFromID] int NOT NULL,
    [AccountToId] int NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transactions_Accounts_AccountFromID] FOREIGN KEY ([AccountFromID]) REFERENCES [Accounts] ([Id]),
    CONSTRAINT [FK_Transactions_Accounts_AccountToId] FOREIGN KEY ([AccountToId]) REFERENCES [Accounts] ([Id])
);
GO

CREATE INDEX [IX_Accounts_CustomerId] ON [Accounts] ([CustomerId]);
GO

CREATE INDEX [IX_Customers_BankId] ON [Customers] ([BankId]);
GO

CREATE INDEX [IX_Transactions_AccountFromID] ON [Transactions] ([AccountFromID]);
GO

CREATE INDEX [IX_Transactions_AccountToId] ON [Transactions] ([AccountToId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230414081327_Initial', N'7.0.5');
GO

COMMIT;
GO