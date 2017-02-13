using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    internal class MusicBrainz
    {
        private static async Task<string> FromWebApi(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "c# api client");

                //var uri = new Uri($"http://musicbrainz.org/ws/2/artist/{artist_id}?inc=releases+media&fmt=json");
                var data = await client.GetStringAsync(uri).ConfigureAwait(false);
                return data;
            }
        }
        public static async Task<List<ArtistReleaseInfo>> GetArtistReleaseInfo(string artist_id)
        {

            var artistReleaseInfo = new List<ArtistReleaseInfo>();

            var uri = new Uri($"http://musicbrainz.org/ws/2/artist/{artist_id}?inc=releases+media&fmt=json&limit=10");
            var data = await FromWebApi(uri).ConfigureAwait(false);

            JObject obj = JObject.Parse(data);
            var releases = obj.SelectToken("releases");

            foreach (var release in releases)
            {
                //var release = releases[i];

                ArtistReleaseInfo info = new ArtistReleaseInfo();

                var Id = release.SelectToken("id")?.ToString();
                var res = await GetLabelInfo(Id, true).ConfigureAwait(false);

                var label = res?.Item1;
                var others = res?.Item2;

                var title = release.SelectToken("title")?.ToString();
                var status = release.SelectToken("status")?.ToString();

                var numberOfTracks = release.SelectToken("media")[0]?.SelectToken("track-count")?.ToString() ?? "0";

                var date = release.SelectToken("date")?.ToString();
                DateTime dt;
                if (DateTime.TryParse(date, out dt))
                {
                    date = dt.Year.ToString();
                }

                info.ReleaseId = Id;
                info.Title = title;
                info.Status = status;
                info.Label = label;
                info.NumberOfTracks = int.Parse(numberOfTracks);
                if (others?.Count > 1)
                    info.OtherArtists.AddRange(others);
                info.YearOfRelease = date;

                artistReleaseInfo.Add(info);
            }

            return artistReleaseInfo.Take(10).ToList();


        }
        private static async Task<Tuple<string, List<OtherArtist>>> GetLabelInfo(string Id, bool retry)
        {
            try
            {
                var uri = new Uri($"http://musicbrainz.org/ws/2/release/{Id}?fmt=json&inc=artist-credits+media+labels");
                var data = await FromWebApi(uri).ConfigureAwait(false);
                JObject obj = JObject.Parse(data);
                List<OtherArtist> others = new List<OtherArtist>();
                var artists = obj.SelectTokens("artist-credit.artist");
                foreach (var art in artists)
                {
                    var otherArtist = new OtherArtist
                    {
                        Id = art.SelectToken("id")?.ToString(),
                        Name = art.SelectToken("name")?.ToString()
                    };
                    // if (!others.Contains(otherArtist))
                    others.Add(otherArtist);
                }

                return new Tuple<string, List<OtherArtist>>(obj.SelectToken("label-info..name")?.ToString(), others);
            }
            catch (Exception)
            {
                if (retry)
                    return await GetLabelInfo(Id, false);
            }

            return null;
        }
    }
}
