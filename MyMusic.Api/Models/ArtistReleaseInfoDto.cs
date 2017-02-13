using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusic.Api.Models
{
    public class ArtistReleaseInfoDto
    {
        public string ReleaseId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Label { get; set; }
        public int NumberOfTracks { get; set; }
        public List<OtherArtistDto> OtherArtists { get; set; }
        public string YearOfRelease { get; set; }
        public List<string> InFavourites { get; set; } = new List<string>();
    }
}