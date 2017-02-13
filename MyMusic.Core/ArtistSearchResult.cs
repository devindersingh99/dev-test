using System.Collections.Generic;

namespace MyMusic.Core
{
    public class ArtistSearchResult
    {
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public int NoOfSearchResults { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int NumberOfPages { get; set; }
    }
}