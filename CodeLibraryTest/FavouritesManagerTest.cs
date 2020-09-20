using System;
using System.Collections.Generic;
using NUnit.Framework;
using Novell.Zenworks.AppModule.Schema;
using Novell.Zenworks.AppModule;
using Novell.Zenworks.Zmd;
using Moq;
using Novell.Zenworks.AppModule.EndUser;
using Novell.Zenworks.AppModule.src.EndUser.Favourite.Util;
using Novell.Zenworks.AppModule.src.schema;

namespace CodeLibraryTest
{

    [TestFixture]
    public class FavouritesManagerTest
    {
        Mock<EndUserSettingDAO> mockEndUserSettingDAO;
        FavouritesManager endUserFavouritesManager;

        [SetUp]
        public void SetUp()
        {
            mockEndUserSettingDAO = new Mock<EndUserSettingDAO>();
            endUserFavouritesManager = new FavouritesManager(mockEndUserSettingDAO.Object);
        }

        [Test]
        public virtual void TestWhenFavouritesInDbIsEmpty()
        {
            //var mockEndUserSettingDAO = new Mock<EndUserSettingDAO>();
            FavouritesData[] favouritesFromDb = new FavouritesData[0];
            FavouritesData[] favouritesFromResponse = new FavouritesData[3];


            FavouritesData favDataFromResponse1 = new FavouritesData();
            favDataFromResponse1.appId = "a101";
            favDataFromResponse1.isFavourite = 1;
            favDataFromResponse1.lastUpdated = "2019-02-08 16:42:27";

            FavouritesData favDataFromResponse2 = new FavouritesData();
            favDataFromResponse2.appId = "b102";
            favDataFromResponse2.isFavourite = 2;
            favDataFromResponse2.lastUpdated = "2019-02-08 17:14:36";

            FavouritesData favDataFromResponse3 = new FavouritesData();
            favDataFromResponse3.appId = "c103";
            favDataFromResponse3.isFavourite = 1;
            favDataFromResponse3.lastUpdated = "2019-02-08 14:58:31";

            favouritesFromResponse[0] = favDataFromResponse1;
            favouritesFromResponse[1] = favDataFromResponse2;
            favouritesFromResponse[2] = favDataFromResponse3;

            mockEndUserSettingDAO.Setup(m => m.SaveOrUpdateNewFavourites(It.IsAny<FavouritesData[]>(), It.IsAny<string>())).Callback((FavouritesData[] favourites, string uid) => {  });
               
            endUserFavouritesManager.SaveOrUpdateNewFavourites(favouritesFromDb, favouritesFromResponse, "UserId1");

            mockEndUserSettingDAO.Verify(m => m.SaveOrUpdateNewFavourites(favouritesFromResponse, "UserId1"), Times.Once());
        }

   

        [Test]
        public virtual void TestWhenFavouritesInDbIsNotEmptyAndSameAsResponse()
        {
            FavouritesData[] favouritesFromDb = new FavouritesData[3];
            FavouritesData[] favouritesFromResponse = new FavouritesData[3];
            FavouritesData[] result = new FavouritesData[0];
          
            FavouritesData favData1 = new FavouritesData();
            favData1.appId = "a101";
            favData1.isFavourite = 1;
            favData1.lastUpdated = "2019-02-08 16:42:27";

            FavouritesData favData2 = new FavouritesData();
            favData2.appId = "b102";
            favData2.isFavourite = 2;
            favData2.lastUpdated = "2019-02-08 17:14:36";

            FavouritesData favData3 = new FavouritesData();
            favData3.appId = "c103";
            favData3.isFavourite = 1;
            favData3.lastUpdated = "2019-02-08 14:58:31";

            favouritesFromDb[0] = favData2;
            favouritesFromDb[1] = favData1;
            favouritesFromDb[2] = favData3;

            favouritesFromResponse[0] = favData1;
            favouritesFromResponse[1] = favData3;
            favouritesFromResponse[2] = favData2;

            mockEndUserSettingDAO.Setup(m => m.SaveOrUpdateNewFavourites(It.IsAny<FavouritesData[]>(), It.IsAny<string>())).Callback((FavouritesData[] favourites, string uid) => { });

            endUserFavouritesManager.SaveOrUpdateNewFavourites(favouritesFromDb, favouritesFromResponse, "UserId1");

            mockEndUserSettingDAO.Verify(m => m.SaveOrUpdateNewFavourites(result, "UserId1"), Times.Never());
        }


        [Test]
        public void TestWhenFavouritesInDbIsNotEmptyAndNotSameAsResponse()
        {
            FavouritesData[] favouritesFromDb = new FavouritesData[1];
            FavouritesData[] favouritesFromResponse = new FavouritesData[3];
            FavouritesData[] result = new FavouritesData[2];

            FavouritesData favData1 = new FavouritesData();
            favData1.appId = "a101";
            favData1.isFavourite = 1;
            favData1.lastUpdated = "2019-02-08 16:42:27";

            FavouritesData favData2 = new FavouritesData();
            favData2.appId = "b102";
            favData2.isFavourite = 2;
            favData2.lastUpdated = "2019-02-08 17:14:36";

            FavouritesData favData3 = new FavouritesData();
            favData3.appId = "c103";
            favData3.isFavourite = 1;
            favData3.lastUpdated = "2019-02-08 14:58:31";

            favouritesFromDb[0] = favData2;
            
            favouritesFromResponse[0] = favData1;
            favouritesFromResponse[1] = favData3;
            favouritesFromResponse[2] = favData2;

            result[0] = favData1;
            result[1] = favData3;

            mockEndUserSettingDAO.Setup(m => m.SaveOrUpdateNewFavourites(It.IsAny<FavouritesData[]>(), It.IsAny<string>())).Callback((FavouritesData[] favourites, string uid) => {  });

            endUserFavouritesManager.SaveOrUpdateNewFavourites(favouritesFromDb, favouritesFromResponse, "UserId1");

            mockEndUserSettingDAO.Verify(m => m.SaveOrUpdateNewFavourites(result, "UserId1"), Times.Once());
        }
 


    }
}
