CREATE TABLE DiscordSyncUser(
	Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Username NVARCHAR(120) NOT NULL,
    Discriminator INT NOT NULL,
    UserId BIGINT NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);

CREATE TABLE [Profile](
	Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Username NVARCHAR(120) NOT NULL,
    Discriminator INT NOT NULL,
    UserId BIGINT NULL,
    Email NVARCHAR(160) NOT NULL,
    CustomerId NVARCHAR(160) NULL,
    SubscriptionType NVARCHAR(160) NOT NULL DEFAULT('FREE'),
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);


CREATE TABLE Alert (
	Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    UserId BIGINT NOT NULL,
    ItemId INT NOT NULL,
    Quantity INT NULL,
    MinPrice INT NULL,
    MaxPrice INT NULL,
    ItemName NVARCHAR(160) NOT NULL,
    ItemNameFr NVARCHAR(160) NOT NULL,
    StartDate DATETIME NULL,
    EndDate DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);
