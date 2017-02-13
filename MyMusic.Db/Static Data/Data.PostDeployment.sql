/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT dbo.Artist ON

INSERT INTO dbo.Artist ([ArtistId],[Name],[UniqueId],[Country])
SELECT 1, N'Metallica'		,'65f4f0c5-ef9e-490c-aee3-909e7ae6b2ab',	N'US'
UNION   
SELECT 2,N'Lady Gaga'		,'650e7db6-b795-4eb5-a702-5ea2fc46c848',	N'US'
UNION   
SELECT 3, N'Mumford & Sons'	,'c44e9c22-ef82-4a77-9bcd-af6c958446d6',	N'GB'
UNION  
SELECT 4,N'Mott the Hoople'	,'435f1441-0f43-479d-92db-a506449a686b',	N'GB'
UNION  
SELECT 5, N'Megadeth'		,'a9044915-8be3-4c7e-b11f-9e2d2ea0a91e',	N'US'
UNION  
SELECT 6, N'John Coltrane'	,'b625448e-bf4a-41c3-a421-72ad46cdb831',	N'US'
UNION  
SELECT 7, N'Mogwai'			,'d700b3f5-45af-4d02-95ed-57d301bda93e',	N'GB'
UNION  
SELECT 8, N'John Mayer'		,'144ef525-85e9-40c3-8335-02c32d0861f3',	N'US'
UNION  
SELECT 9, N'Johnny Cash'		,'18fa2fd5-3ef2-4496-ba9f-6dae655b2a4f',	N'US'
UNION  
SELECT 10, N'Jack Johnson'	,'6456a893-c1e9-4e3d-86f7-0008b0a3ac8a',	N'US'
UNION  
SELECT 11,N'John Frusciante'	,'f1571db1-c672-4a54-a2cf-aaa329f26f0b',	N'US'
UNION  
SELECT 12,N'Elton John'		,'b83bc61f-8451-4a5d-8b8e-7e9ed295e822',	N'GB'
UNION  
SELECT 13, N'Rancid'			,'24f8d8a5-269b-475c-a1cb-792990b0b2ee',	N'US'
UNION  
SELECT 14, N'Transplants'		,'29f3e1bf-aec1-4d0a-9ef3-0cb95e8a3699',	N'US'
UNION  
SELECT 15,N'Operation Ivy'	,'931e1d1f-6b2f-4ff8-9f70-aa537210cd46',	N'US'

SET IDENTITY_INSERT dbo.Artist OFF

GO

INSERT INTO dbo.ArtistAlias ([ArtistId],[Alias])
SELECT 1, N'Metalica'
UNION
SELECT 1, N'메탈리카'
UNION
SELECT 2, N'Lady Ga Ga'
UNION
SELECT 2, N'Stefani Joanne Angelina Germanotta'
UNION
SELECT 4, N'Mott The Hoppie' 
UNION
SELECT 4 ,N'Mott The Hopple'
UNION
SELECT 5, N'Megadeath'
UNION
SELECT 6, N'John Coltraine'
UNION
SELECT 6, N'John William Coltrane'
UNION
SELECT 7, N'Mogwa'
UNION
SELECT 9, N'Johhny Cash' 
UNION
SELECT 9, N'Jonny Cash'
UNION
SELECT 10,N'Jack Hody Johnson'
UNION
SELECT 11,N'John Anthony Frusciante'
UNION
SELECT 12,N'E. John'
UNION
SELECT 12, N'Elthon John'
UNION
SELECT 12,N'Elton Jphn'
UNION
SELECT 12,N'John Elton'
UNION
SELECT 12, N'Reginald Kenneth Dwight'
UNION
SELECT 13,N'ランシド'
UNION
SELECT 14,N'The Transplants'
UNION
SELECT 15,N'Op Ivy'

GO


