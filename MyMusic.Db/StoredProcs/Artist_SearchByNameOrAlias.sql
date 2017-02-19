
CREATE PROCEDURE [dbo].[Artist_SearchByNameOrAlias]
	@SearchText NVARCHAR(256),
	@Page INT = 1,
	@Page_Size INT = 10
AS
	WITH Cte AS
	(
	SELECT
		ArtistId,
		Name,
		UniqueId,
		Country,
		ArtistAlias,
		ROW_NUMBER() OVER (ORDER BY ArtistId) as RowNum
	FROM
		dbo.Artist 
		WHERE Name LIKE @SearchText + '%' OR ArtistAlias LIKE  @SearchText + '%' OR ArtistAlias LIKE '%,' + @SearchText + '%'
	)
	SELECT
		ArtistId,
		Name,
		UniqueId,
		Country,
		ArtistAlias,
		RowNum,
		(Select max(RowNum) from cte) as NoOfRecords,
		@page,
		@page_size
	FROM 
	Cte
	ORDER BY Rownum
	offset (@page-1) * @page_size rows 
	fetch next @page_size row only
