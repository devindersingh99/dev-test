using AutoMapper;
using MyMusic.Api.App_Start;
using MyMusic.Api.Models;
using MyMusic.Core;
using MyMusic.Core.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyMusic.Api.Controllers
{
    [RoutePrefix("artist")]
    public class ArtistController : ApiController
    {

        public IArtistRepository ArtistRepository { get; }
        public IFavouriteRepository FavouriteRepository;

        public ArtistController(IArtistRepository artistRepository, IFavouriteRepository favouriteRepository)
        {
            ArtistRepository = artistRepository;
            FavouriteRepository = favouriteRepository;
        }
        [Route("search/{search_criteria}/{page_number}/{page_size}")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string search_criteria, int page_number, int page_size)
        {
            if (string.IsNullOrWhiteSpace(search_criteria))
                return BadRequest($"{nameof(search_criteria)} can't be empty");

            if (page_size < 1)
                return BadRequest($"{page_size} can't be less than 1");

            if (page_number < 1)
                return BadRequest($"{page_number} can't be less than 1");

            var result = await ArtistRepository.SearchByNameOrAlias(search_criteria, page_number, page_size);
            var dtoResult = AutoMapperConfig.Mapper.Map<ArtistSearchResultDto>(result);
            UpdateReleasesUrls(dtoResult);
            return Ok(dtoResult);

        }

        private void UpdateReleasesUrls(ArtistSearchResultDto resultsDto)
        {
            foreach (var resultDto in resultsDto.Results)
            {
                resultDto.ReleaseUrl = new Uri(this.Url.Link("ReleasesApi", new { artist_id = resultDto.Uid }));
            }
        }

        [Route("{artist_id}/releases", Name = "ReleasesApi")]
        [HttpGet]
        public async Task<IHttpActionResult> Releases(string artist_id)
        {
            if (string.IsNullOrEmpty(artist_id))
                return BadRequest($"{nameof(artist_id)} can't be empty");

            var result = await ArtistRepository.GetArtistReleasesInfoAsync(artist_id).ConfigureAwait(false);
            var resultDto = AutoMapperConfig.Mapper.Map<List<ArtistReleaseInfoDto>>(result);
            UpdateInFavourites(resultDto);
            return Ok(resultDto);
        }

        private void UpdateInFavourites(List<ArtistReleaseInfoDto> artistInfoDtos)
        {
            foreach (var dto in artistInfoDtos)
            {
                dto.InFavourites.AddRange(FavouriteRepository.FavouriteNameFromReleaseInfoId(dto.ReleaseId));
            }

        }
    }
}
