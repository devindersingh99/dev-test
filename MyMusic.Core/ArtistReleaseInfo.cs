using System;
using System.Collections.Generic;

namespace MyMusic.Core
{
    public class ArtistReleaseInfo
    {
        public string ReleaseId{ get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Label { get; set; }
        public int NumberOfTracks { get; set; }
        public List<OtherArtist> OtherArtists { get; set; } = new List<OtherArtist>();
        public string YearOfRelease { get; set; }
    }
}