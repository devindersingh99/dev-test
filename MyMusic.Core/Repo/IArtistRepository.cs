using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusic.Core.Repo
{
    public interface IArtistRepository
    {
        Task<ArtistSearchResult> SearchByNameOrAlias(string search_criteria, int page_number, int page_Size);
        Task<List<ArtistReleaseInfo>> GetArtistReleasesInfoAsync(string artist_id);
    }
}