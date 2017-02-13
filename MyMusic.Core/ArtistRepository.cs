using Dapper;
using MyMusic.Core.Repo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public class ArtistRepository : IArtistRepository
    {
        public IDatabaseConnectionFactory ConnectionFactory { get; }
        public ArtistRepository(IDatabaseConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }
        public async Task<ArtistSearchResult> SearchByNameOrAlias(string search_criteria, int page_number, int page_Size)
        {
            IEnumerable<Artist> artists = null;
            PagingInfo pagingInfo = null;

            using (IDbConnection con = ConnectionFactory.GetConnection())
            {
                var multiResults = await con.QueryMultipleAsync("dbo.Artist_SearchByNameOrAlias",
                    new { SearchText = search_criteria, PageNumber = page_number, PageSize = page_Size },
                    commandType: System.Data.CommandType.StoredProcedure);

                artists = multiResults.Read<Artist>();
                pagingInfo = multiResults.Read<PagingInfo>().FirstOrDefault();
            }

            foreach (Artist artist in artists)
            {
                if (artist.Alias != null)
                    artist.AliasList.AddRange(artist.Alias.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(alias => alias.Trim()));
            }

            var results = new ArtistSearchResult()
            {
                Artists = artists.ToList(),
                Page = pagingInfo.Page,
                PageSize = pagingInfo.pageSize,
                NumberOfPages = pagingInfo.NumberOfPages,
                NoOfSearchResults = pagingInfo.NumberOfSearchResults
            };


            return results;
        }
        public async Task<List<ArtistReleaseInfo>> GetArtistReleasesInfoAsync(string artist_id)
        {
            return await MusicBrainz.GetArtistReleaseInfo(artist_id);
        }

    }
}
