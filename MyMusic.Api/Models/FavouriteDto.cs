using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMusic.Api.Models
{
    public class FavouriteDto
    {
        public string Name { get; set; }
        public List<FavouriteReleaseInfoDto> Releases { get; set; }
    }
}