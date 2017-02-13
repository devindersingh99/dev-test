using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusic.Api.Models
{
    public class FavouriteReleaseInfoDto
    {
        public string ReleaseId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
    }
}