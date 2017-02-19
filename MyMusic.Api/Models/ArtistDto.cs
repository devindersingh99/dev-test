using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusic.Api.Models
{
    public class ArtistDto
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        //public List<string> Alias { get; set; }
        public Uri ReleaseUrl { get; set; }
        public string ArtistAlias { get; set; }
    }
}