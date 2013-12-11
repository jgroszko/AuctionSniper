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
    public class AuctionSniperEndToEndTests
    {
        static string auctionId = "54321";

        [TestMethod]
        public void SniperJoinsAuctionUntilAuctionCloses()
        {
            using(AuctionSniperDriver application = new AuctionSniperDriver())
            using(var auction = new FakeAuctionServer(auctionId))
            {
                auction.StartSellingItem();
                application.StartBiddingIn(auctionId);
                auction.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);
                auction.AnnounceClosed();
                application.ShowsSniperHasLostAuction();
            }
        }


        [TestMethod]
        public void SniperMakesAHigherBidButLoses()
        {
            using (AuctionSniperDriver application = new AuctionSniperDriver())
            using (var auction = new FakeAuctionServer(auctionId))
            {
                auction.StartSellingItem();
                application.StartBiddingIn(auctionId);

                auction.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);
                auction.ReportPrice(1000, 98, "other bidder");
                
                application.HasShownSniperIsBidding();
                auction.HasReceivedBid(1098, AuctionSniperDriver.SNIPER_ID);

                auction.AnnounceClosed();
                application.ShowsSniperHasLostAuction();
            }
        }
    }
}
