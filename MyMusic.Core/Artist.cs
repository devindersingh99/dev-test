using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public class Artist
    {
        public Guid UniqueId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        //public List<string> AliasList { get; set; } = new List<string>();
        public string ArtistAlias { get; set; }
        

    }
}
