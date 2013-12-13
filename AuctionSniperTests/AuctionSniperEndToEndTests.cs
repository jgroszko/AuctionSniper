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

        AuctionSniperDriver application;
        FakeAuctionServer auction;

        [TestInitialize]
        public void Initialize()
        {
            application = new AuctionSniperDriver();
            auction = new FakeAuctionServer(auctionId);
        }

        [TestCleanup]
        public void Cleanup()
        {
            auction.Dispose();
            application.Dispose();
        }

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

        [TestMethod]
        public void SniperWinsAnAuctionByBiddingHigher()
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

                auction.ReportPrice(1098, 97, AuctionSniperDriver.SNIPER_ID);
                application.HasShowSniperIsWinning();

                auction.AnnounceClosed();
                application.ShowsSniperHasWonAuction();
            }
        }
    }
}
