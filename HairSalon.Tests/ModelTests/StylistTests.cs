using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using HairSalon.Models;

namespace HairSalon.Tests
{
  [TestClass]
  public class StylistTests : IDisposable
  {
        public StylistTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889 ;database=sravanthi_velaga_test;";
        }

        public void Dispose()
        {
          Client.DeleteAll();
          Stylist.DeleteAll();
          // Review.DeleteAll();
        }

       [TestMethod]
       public void GetAll_StylistEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Stylist.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Stylist()
      {
        //Arrange, Act
        Stylist firstStylist = new Stylist("Richard");
        Stylist secondStylist = new Stylist("Richard");

        //Assert
        Assert.AreEqual(firstStylist, secondStylist);
      }

      [TestMethod]
      public void Save_SavesStylistToDatabase_StylistList()
      {
        //Arrange
        Stylist testStylist = new Stylist("Richard");
        testStylist.Save();

        //Act
        List<Stylist> result = Stylist.GetAll();
        List<Stylist> testList = new List<Stylist>{testStylist};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }


     [TestMethod]
     public void Save_DatabaseAssignsIdToStylist_Id()
     {
       //Arrange
       Stylist testStylist = new Stylist("Richard");
       testStylist.Save();

       //Act
       Stylist savedStylist = Stylist.GetAll()[0];

       int result = savedStylist.GetId();
       int testId = testStylist.GetId();

       //Assert
       Assert.AreEqual(testId, result);
    }


    [TestMethod]
    public void Find_FindsStylistInDatabase_Stylist()
    {
      //Arrange
      Stylist testStylist = new Stylist("Richard");
      testStylist.Save();

      //Act
      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      //Assert
      Assert.AreEqual(testStylist, foundStylist);
    }

    [TestMethod]
    public void GetClients_RetrievesAllClientsWithStylist_ClientList()
    {
      Stylist testStylist = new Stylist("Richard");
      testStylist.Save();

      Client firstClient = new Client("Richard", testStylist.GetId());
      firstClient.Save();
      Client secondClient = new Client("Mary", testStylist.GetId());
      secondClient.Save();


      List<Client> testClientList = new List<Client> {firstClient, secondClient};
      List<Client> resultClientList = testStylist.GetClients();

      CollectionAssert.AreEqual(testClientList, resultClientList);
    }

  }
}
