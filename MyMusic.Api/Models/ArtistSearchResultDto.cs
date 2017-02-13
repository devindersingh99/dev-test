using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusic.Api.Models
{
    public class ArtistSearchResultDto
    {
        public int NumberOfSearchResults { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int NumberOfPages { get; set; }
        public List<ArtistDto> Results { get; set; }
    }
}