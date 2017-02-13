CREATE TABLE [dbo].[ArtistAlias]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[ArtistId] INT NOT NULL,
	[Alias] NVARCHAR(1024) NOT NULL,
	CONSTRAINT PK_ArtistAlias_Id PRIMARY KEY ([Id]),
	CONSTRAINT FK__ArtistAlias_Artist_Id_Artist_ArtistId FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artist]([ArtistId])
)
GO
CREATE NONCLUSTERED INDEX [IX_ArtistAlias_Alias]  ON [dbo].[ArtistAlias]([Alias])

