using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyMusic.Api.App_Start;
using MyMusic.Api.Controllers;
using MyMusic.Api.Models;
using MyMusic.Core;
using MyMusic.Core.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace MyMusic.Api.Tests
{
    [TestClass]
    public class ArtistControllerTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod]
        public void Releases_ReturnsOneResult()
        {

            var mockArtistRepo = new Mock<IArtistRepository>();
            var mockFavouriteRepo = new Mock<IFavouriteRepository>();
            mockArtistRepo.Setup(repo => repo.GetArtistReleasesInfoAsync("1")).Returns(GetArtistRelesese());
            mockFavouriteRepo.Setup(repo => repo.FavouriteNameFromReleaseInfoId(It.IsAny<string>())).Returns(new List<string>());
            ArtistController artistController = new ArtistController(mockArtistRepo.Object, mockFavouriteRepo.Object);
            var result = artistController.Releases("1").Result as OkNegotiatedContentResult<List<ArtistReleaseInfoDto>>;

            Assert.AreEqual(1, result.Content.Count);
            
        }

        private Task<List<ArtistReleaseInfo>> GetArtistRelesese()
        {
            return Task.FromResult(new List<ArtistReleaseInfo>()
            {
                 new ArtistReleaseInfo()
                 {
                     Label ="HMV",
                     NumberOfTracks =10,
                     ReleaseId ="123",
                     Status ="Official",
                     Title = "Best of me",
                     YearOfRelease ="1975" ,
                     OtherArtists = new List<OtherArtist>()
                 }
            });
        }
    }
}
