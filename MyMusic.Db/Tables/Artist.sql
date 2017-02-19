CREATE TABLE [dbo].[Artist]
(
	[ArtistId] INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(32) NOT NULL,
	[UniqueId] UNIQUEIDENTIFIER NOT NULL,
	[Country] NCHAR(2) NOT NULL,
	[ArtistAlias] NVARCHAR(100)
	CONSTRAINT PK_Artist_ArtistId PRIMARY KEY ([ArtistId])
)
GO
CREATE NONCLUSTERED INDEX [IX_Artist_Name] ON [dbo].[Artist]([Name])
