CREATE TABLE [dbo].[Athletes] (
    [Athlete_ID] INT          IDENTITY (1, 1) NOT NULL,
    [LastName]   VARCHAR (50) NULL,
    [FirstName]  VARCHAR (50) NULL,
    [Sports]     VARCHAR (50) NULL,
    [TeamName]   VARCHAR (50) NULL,
    [phone]      INT          NULL,
    PRIMARY KEY CLUSTERED ([Athlete_ID] ASC)
);

CREATE TABLE [dbo].[Injury] (
    [Injury_ID]    INT           IDENTITY (1, 1) NOT NULL,
    [Athlete_ID]   INT           NOT NULL,
    [InjuryType]   VARCHAR (100) NOT NULL,
    [StartDate]    DATE          NOT NULL,
    [RecoveryDate] DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([Injury_ID] ASC)
);

CREATE TABLE [dbo].[Sports] (
    [Sport_ID]      INT          IDENTITY (1, 1) NOT NULL,
    [SportName]     VARCHAR (50) NULL,
    [LeagueName]    VARCHAR (50) NULL,
    [TeamsCount]    INT          NULL,
    [ManagersCount] INT          NULL,
    [AthletesCount] INT          NULL,
    PRIMARY KEY CLUSTERED ([Sport_ID] ASC)
);

CREATE TABLE [dbo].[Team] (
    [Team_ID]            INT          IDENTITY (1, 1) NOT NULL,
    [TeamName]           VARCHAR (50) NULL,
    [LeagueName]         VARCHAR (50) NULL,
    [InjuredPlayerCount] INT          NULL,
    PRIMARY KEY CLUSTERED ([Team_ID] ASC)
);

CREATE TABLE [dbo].[Athletes_Injury_Bridge] (
    [link_ID]    INT IDENTITY (1, 1) NOT NULL,
    [Athlete_ID] INT NULL,
    [Injury_ID]  INT NULL,
    PRIMARY KEY CLUSTERED ([link_ID] ASC),
    FOREIGN KEY ([Athlete_ID]) REFERENCES [dbo].[Athletes] ([Athlete_ID]),
    FOREIGN KEY ([Injury_ID]) REFERENCES [dbo].[Injury] ([Injury_ID])
);