using System;

namespace MyMusic.Core
{
    
        public class OtherArtist : IEquatable<OtherArtist>
        {
            public string Id { get; set; }
            public string Name { get; set; }

        public bool Equals(OtherArtist other)
        {
            if (other == null)
                return false;

            return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
        }
    }
}