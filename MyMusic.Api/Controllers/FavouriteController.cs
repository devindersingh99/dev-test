using System.Web.Http;
using MyMusic.Core.Repo;
using MyMusic.Api.App_Start;
using System.Collections.Generic;
using MyMusic.Api.Models;
using MyMusic.Core;

namespace MyMusic.Api.Controllers
{
    [RoutePrefix("favourite")]
    public class FavouriteController : ApiController
    {
        public IFavouriteRepository FavouriteRepository { get; }

        public FavouriteController(IFavouriteRepository favouriteRepository)
        {
            FavouriteRepository = favouriteRepository;
        }
        [Route("get")]
        [HttpGet]
        public IHttpActionResult GetFavourites()
        {
            var result = FavouriteRepository.GetFavourites();
            var resultDto = GetFavouriteDto(result);
            return Ok(resultDto);

        }

        private List<FavouriteDto> GetFavouriteDto(Dictionary<string, List<FavouriteReleaseInfo>> favouritesInfo)
        {
            List<FavouriteDto> favourites = new List<FavouriteDto>();
            foreach (var fav in favouritesInfo)
            {
                FavouriteDto f = new FavouriteDto();
                f.Name = fav.Key;
                f.Releases = AutoMapperConfig.Mapper.Map<List<FavouriteReleaseInfoDto>>(favouritesInfo[fav.Key]);
                favourites.Add(f);
            }

            return favourites;
        }


        [Route("get/{name}")]
        [HttpGet]
        public IHttpActionResult GetFavouritesByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest($"{nameof(name)} can't be empty ");

            var result = FavouriteRepository.GetFavourite(name);
            var resultDto = AutoMapperConfig.Mapper.Map<Dictionary<string, List<ArtistReleaseInfoDto>>>(result);
            return Ok(resultDto);

        }

        [Route("createlist/{name}")]
        [HttpPost]
        public IHttpActionResult CreateFavouriteList(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest($"{nameof(name)} can't be empty ");

            if (FavouriteRepository.FavouriteExists(name))
                return Conflict();

            FavouriteRepository.CreateFavouriteList(name);
            return Ok();
        }

        [Route("deletelist/{name}")]
        [HttpDelete]
        public IHttpActionResult DeleteFavouriteList(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest($"{nameof(name)} can't be empty ");

            FavouriteRepository.DeleteFavouriteList(name);
            return Ok();
        }

        [Route("addrelease/{name}/{releaseId}/{title}/{label}")]
        [HttpPost]
        public IHttpActionResult AddReleaseToFavourite(string name, string releaseId, string title, string label)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest($"{nameof(name)} can't be empty ");

            if (string.IsNullOrEmpty(releaseId))
                return BadRequest($"{nameof(releaseId)} can't be empty ");

            FavouriteReleaseInfoDto favDto = new FavouriteReleaseInfoDto { ReleaseId = releaseId, Label = label, Title = title };

            var releaseInfo = AutoMapperConfig.Mapper.Map<FavouriteReleaseInfo>(favDto);

            FavouriteRepository.AddToFavourite(name, releaseInfo);

            return Ok();

        }

        [Route("list")]
        [HttpGet]
        public IHttpActionResult FavouriteListName()
        {
            return Ok(FavouriteRepository.GetFavouriteListName());
        }

        [Route("remove/{name}/{releaseId}")]
        [HttpPost]
        public IHttpActionResult RemoveReleaseFromFavourites(string name, string releaseId)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest($"{nameof(name)} can't be empty ");

            if (string.IsNullOrEmpty(releaseId))
                return BadRequest($"{nameof(releaseId)} can't be empty ");

            FavouriteRepository.RemoveFromFavourite(name, releaseId);

            return Ok();
        }



    }
}
