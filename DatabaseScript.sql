USE [master]
GO
/****** Object:  Database [Music]    Script Date: 15/02/2017 20:24:57 ******/
CREATE DATABASE [Music]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Music', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQL2012\MSSQL\DATA\Music_Primary.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Music_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQL2012\MSSQL\DATA\Music_Primary.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Music] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Music].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Music] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [Music] SET ANSI_NULLS ON 
GO
ALTER DATABASE [Music] SET ANSI_PADDING ON 
GO
ALTER DATABASE [Music] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [Music] SET ARITHABORT ON 
GO
ALTER DATABASE [Music] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Music] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Music] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Music] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Music] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Music] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [Music] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [Music] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Music] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [Music] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Music] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Music] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Music] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Music] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Music] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Music] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Music] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Music] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Music] SET RECOVERY FULL 
GO
ALTER DATABASE [Music] SET  MULTI_USER 
GO
ALTER DATABASE [Music] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [Music] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Music] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Music] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Music', N'ON'
GO
USE [Music]
GO
/****** Object:  StoredProcedure [dbo].[Artist_SearchByNameOrAlias]    Script Date: 15/02/2017 20:24:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Artist_SearchByNameOrAlias]
	@SearchText NVARCHAR(256),
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
	DECLARE @Results AS TABLE (UniqueID  UNIQUEIDENTIFIER,Name  NVARCHAR(1024), Country  NVARCHAR(2), Alias  NVARCHAR(1024),Row_Num INT )

	;With ArtistCte as	
	(
		SELECT DISTINCT
		AR.UniqueId,
		AR.Name ,
		AR.Country,
		Alias = (SELECT STUFF((SELECT ', ' + [Alias] FROM ArtistAlias AA WHERE AA.ArtistId = AR.ArtistId FOR XML PATH('')),1,1,'') as Alias)
	
		FROM dbo.Artist AR
		LEFT JOIN dbo.ArtistAlias AA ON AR.ArtistId = AA.ArtistId
		WHERE AR.Name LIKE @searchText + '%' OR AA.Alias LIKE @SearchText + '%'

	)
	INSERT INTO @Results
	SELECT
		UniqueID,
		Name,
		Country,
		Alias,
		ROW_NUMBER() OVER (Order by Name) as Row_Num
	From
		ArtistCte
	
	
	SELECT
		UniqueID,
		Name,
		Country,
		Alias
	FROM @Results
	WHERE Row_Num > (@PageNumber -1) * @PageSize AND Row_Num <= (@PageNumber) * @PageSize

	DECLARE @Max_Rows  INT= (Select Max(Row_Num) From @Results);

	SELECT
	'NumberOfSearchResults' = ISNULL(@Max_Rows,0),
	'Page' = @PageNumber,
	'PageSize' = @PageSize ,
	'NumberOfPages' = ISNULL(CASE @Max_Rows % @PageSize 
						WHEN  0 THEN (@Max_Rows / @PageSize )
						ELSE (@Max_Rows / @PageSize) + 1 
						END ,0)

	


GO
/****** Object:  Table [dbo].[Artist]    Script Date: 15/02/2017 20:24:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artist](
	[ArtistId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](1024) NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Country] [nchar](2) NOT NULL,
 CONSTRAINT [PK_Artist_ArtistId] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArtistAlias]    Script Date: 15/02/2017 20:24:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtistAlias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArtistId] [int] NOT NULL,
	[Alias] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_ArtistAlias_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Artist] ON 

GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (1, N'Metallica', N'65f4f0c5-ef9e-490c-aee3-909e7ae6b2ab', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (2, N'Lady Gaga', N'650e7db6-b795-4eb5-a702-5ea2fc46c848', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (3, N'Mumford & Sons', N'c44e9c22-ef82-4a77-9bcd-af6c958446d6', N'GB')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (4, N'Mott the Hoople', N'435f1441-0f43-479d-92db-a506449a686b', N'GB')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (5, N'Megadeth', N'a9044915-8be3-4c7e-b11f-9e2d2ea0a91e', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (6, N'John Coltrane', N'b625448e-bf4a-41c3-a421-72ad46cdb831', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (7, N'Mogwai', N'd700b3f5-45af-4d02-95ed-57d301bda93e', N'GB')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (8, N'John Mayer', N'144ef525-85e9-40c3-8335-02c32d0861f3', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (9, N'Johnny Cash', N'18fa2fd5-3ef2-4496-ba9f-6dae655b2a4f', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (10, N'Jack Johnson', N'6456a893-c1e9-4e3d-86f7-0008b0a3ac8a', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (11, N'John Frusciante', N'f1571db1-c672-4a54-a2cf-aaa329f26f0b', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (12, N'Elton John', N'b83bc61f-8451-4a5d-8b8e-7e9ed295e822', N'GB')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (13, N'Rancid', N'24f8d8a5-269b-475c-a1cb-792990b0b2ee', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (14, N'Transplants', N'29f3e1bf-aec1-4d0a-9ef3-0cb95e8a3699', N'US')
GO
INSERT [dbo].[Artist] ([ArtistId], [Name], [UniqueId], [Country]) VALUES (15, N'Operation Ivy', N'931e1d1f-6b2f-4ff8-9f70-aa537210cd46', N'US')
GO
SET IDENTITY_INSERT [dbo].[Artist] OFF
GO
SET IDENTITY_INSERT [dbo].[ArtistAlias] ON 

GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (1, 1, N'Metalica')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (2, 1, N'메탈리카')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (3, 2, N'Lady Ga Ga')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (4, 2, N'Stefani Joanne Angelina Germanotta')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (5, 4, N'Mott The Hoppie')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (6, 4, N'Mott The Hopple')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (7, 5, N'Megadeath')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (8, 6, N'John Coltraine')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (9, 6, N'John William Coltrane')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (10, 7, N'Mogwa')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (11, 9, N'Johhny Cash')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (12, 9, N'Jonny Cash')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (13, 10, N'Jack Hody Johnson')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (14, 11, N'John Anthony Frusciante')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (15, 12, N'E. John')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (16, 12, N'Elthon John')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (17, 12, N'Elton Jphn')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (18, 12, N'John Elton')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (19, 12, N'Reginald Kenneth Dwight')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (20, 13, N'ランシド')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (21, 14, N'The Transplants')
GO
INSERT [dbo].[ArtistAlias] ([Id], [ArtistId], [Alias]) VALUES (22, 15, N'Op Ivy')
GO
SET IDENTITY_INSERT [dbo].[ArtistAlias] OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Artist_Name]    Script Date: 15/02/2017 20:24:57 ******/
CREATE NONCLUSTERED INDEX [IX_Artist_Name] ON [dbo].[Artist]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ArtistAlias_Alias]    Script Date: 15/02/2017 20:24:57 ******/
CREATE NONCLUSTERED INDEX [IX_ArtistAlias_Alias] ON [dbo].[ArtistAlias]
(
	[Alias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArtistAlias]  WITH CHECK ADD  CONSTRAINT [FK__ArtistAlias_Artist_Id_Artist_ArtistId] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artist] ([ArtistId])
GO
ALTER TABLE [dbo].[ArtistAlias] CHECK CONSTRAINT [FK__ArtistAlias_Artist_Id_Artist_ArtistId]
GO
USE [master]
GO
ALTER DATABASE [Music] SET  READ_WRITE 
GO
