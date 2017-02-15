using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusic.Api.Controllers;
using Moq;
using MyMusic.Core.Repo;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Linq;
using MyMusic.Api.App_Start;
using MyMusic.Api.Models;

namespace MyMusic.Api.Tests
{
    [TestClass]
    public class FavouriteControllerTest
    {

        [TestInitialize]
        public void TestInitialize()
        {
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod]
        public void CreateFavouriteList_ReturnsBadRequest()
        {
            var mockRepo = new Mock<IFavouriteRepository>();

            FavouriteController favController = new FavouriteController(mockRepo.Object);
            var result = favController.CreateFavouriteList(null);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            
        }

        [TestMethod]
        public void CreateFavouriteList_FavouriteListExists_ReturnsConflict()
        {
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.FavouriteExists("default")).Returns(true);
            FavouriteController favController = new FavouriteController(mockRepo.Object);
            var result = favController.CreateFavouriteList("default");
            Assert.IsInstanceOfType(result, typeof(ConflictResult));

        }

        [TestMethod]
        public void CreateFavouriteList_CallsCreateFavouriteListOnRepo()
        {
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.FavouriteExists("default")).Returns(false);
            mockRepo.Setup(repo => repo.CreateFavouriteList("default")).Verifiable();

            FavouriteController favController = new FavouriteController(mockRepo.Object);
            var result = favController.CreateFavouriteList("default");

            mockRepo.Verify();

        }

        [TestMethod]
        public void DeleteFavouriteList_CallsDeleteFavouriteListOnRepo()
        {
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.DeleteFavouriteList("default")).Verifiable();

            FavouriteController favController = new FavouriteController(mockRepo.Object);
            var result = favController.DeleteFavouriteList("default");
        }

        [TestMethod]
        public void FavouriteListName_Return_FavouriteListNamedDefault()
        {
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.GetFavouriteListName()).Returns(new List<string> { "default" });
            FavouriteController favController = new FavouriteController(mockRepo.Object);
            var result = favController.FavouriteListName() as System.Web.Http.Results.OkNegotiatedContentResult<List<string>>;

            Assert.IsTrue(result.Content.First() == "default");

        }

        [TestMethod]
        public void RemoveReleaseFromFavourites_CallsRemoveFromFavouriteOnRepo()
        {
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.RemoveFromFavourite("default", "id")).Verifiable();
            FavouriteController favController = new FavouriteController(mockRepo.Object);
            favController.RemoveReleaseFromFavourites("default", "id");

            mockRepo.Verify();

        }

        [TestMethod]
        public void AddReleaseToFavourite_CallsAddToFavouriteOnRepo()
        {
            
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.AddToFavourite("name", It.IsAny<FavouriteReleaseInfo>())).Verifiable(); ;
            FavouriteController favController = new FavouriteController(mockRepo.Object);
            favController.AddReleaseToFavourite("name", "releaseId", "title", "label");

            mockRepo.Verify();
        }

        [TestMethod]
        public void GetFavouritesByName_CallsGetFavouriteOnRepo()
        {
            
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.GetFavourite("name")).Verifiable();
            FavouriteController favController = new FavouriteController(mockRepo.Object);
            favController.GetFavouritesByName("name");

            mockRepo.Verify();

        }

        [TestMethod]
        public void GetFavourites_ReturnFavoutiteListWithOneItem()
        {
            
            var mockRepo = new Mock<IFavouriteRepository>();
            mockRepo.Setup(repo => repo.GetFavourites()).Returns(GetFavourite());
            FavouriteController favController = new FavouriteController(mockRepo.Object);
            var result = favController.GetFavourites() as OkNegotiatedContentResult<List<FavouriteDto>>;

            Assert.AreEqual(1, result.Content.Count);

        }

        private Dictionary<string, List<FavouriteReleaseInfo>> GetFavourite()
        {
            return new Dictionary<string, List<FavouriteReleaseInfo>> {
                { "default", new List<FavouriteReleaseInfo> { new FavouriteReleaseInfo { Label="HMV", ReleaseId="1", Title ="Disco Dance" } } }
            };
        }

}
}
