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
                //var multiResults = await con.QueryMultipleAsync("dbo.Artist_SearchByNameOrAlias",
                //    new { SearchText = search_criteria, PageNumber = page_number, PageSize = page_Size },
                //    commandType: System.Data.CommandType.StoredProcedure);

                //artists = multiResults.Read<Artist>();
                //pagingInfo = multiResults.Read<PagingInfo>().FirstOrDefault();

                var res = con.Query<Artist, PagingInfo, PageArtistInfo>("dbo.Artist_SearchByNameOrAlias",
                    (artist, pageInfo) =>
                    {
                        return new PageArtistInfo { Artist = artist, PageInfo = pageInfo };
                    },
                    new { SearchText = "Joh", Page = 1, Page_Size = 10 },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "NoOfRecords");

                artists = res.Select(r => r.Artist);
                pagingInfo = res.Select(r => r.PageInfo).FirstOrDefault();
                if (pagingInfo.NoOfRecords > 0)
                {
                    pagingInfo.NumberOfPages = pagingInfo.NoOfRecords / pagingInfo.Page_Size;
                    if (pagingInfo.NoOfRecords % pagingInfo.Page_Size > 0)
                        pagingInfo.NumberOfPages += 1;
                }

            }

            //foreach (Artist artist in artists)
            //{
            //    if (artist.Alias != null)
            //        artist.AliasList.AddRange(artist.Alias.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                    .Select(alias => alias.Trim()));
            //}

            var results = new ArtistSearchResult()
            {
                Artists = artists.ToList(),
                Page = pagingInfo.Page,
                PageSize = pagingInfo.Page_Size,
                NumberOfPages = pagingInfo.NumberOfPages,
                NoOfSearchResults = pagingInfo.NoOfRecords
            };


            return results;
        }
        public async Task<List<ArtistReleaseInfo>> GetArtistReleasesInfoAsync(string artist_id)
        {
            return await MusicBrainz.GetArtistReleaseInfo(artist_id);
        }

    }

    class PageArtistInfo
    {
        public Artist Artist { get; set; }
        public PagingInfo PageInfo { get; set; }

    }

}
