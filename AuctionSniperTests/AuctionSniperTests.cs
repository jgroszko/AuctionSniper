using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TestStack.White.ScreenObjects.Sessions;
using System.Reflection;
using TestStack.White.ScreenObjects.Services;
using TestStack.White.Factory;
using TestStack.White;

namespace AuctionSniperTests
{
    [TestClass]
    public class AuctionSniperTests
    {
        [TestMethod]
        public void AuctionSniperEndToEndTest()
        {
            string auctionId = "54321";

            using(AuctionSniperDriver application = new AuctionSniperDriver())
            using(var auction = new FakeAuctionServer(auctionId))
            {
                auction.StartSellingItem();
                application.StartBiddingIn(auctionId);
                auction.hasReceivedJoinRequestFromSniper();
                auction.announceClosed();
                application.showsSniperHasLostAuction();
            }
        }
    }
}
