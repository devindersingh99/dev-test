
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

	

