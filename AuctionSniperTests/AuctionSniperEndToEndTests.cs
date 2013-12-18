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
        AuctionSniperDriver application;
        FakeAuctionServer auction;
        FakeAuctionServer auction2;

        [TestInitialize]
        public void Initialize()
        {
            application = new AuctionSniperDriver();
            auction = new FakeAuctionServer("54321");
            auction2 = new FakeAuctionServer("65432");
        }

        [TestCleanup]
        public void Cleanup()
        {
            auction.Dispose();
            auction2.Dispose();
            application.Dispose();
        }

        [TestMethod]
        public void SniperJoinsAuctionUntilAuctionCloses()
        {
            auction.StartSellingItem();
            application.StartBiddingIn(auction);
            auction.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);
            auction.AnnounceClosed();
            application.ShowsSniperHasLostAuction(auction);
        }


        [TestMethod]
        public void SniperMakesAHigherBidButLoses()
        {
            auction.StartSellingItem();

            application.StartBiddingIn(auction);
            auction.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);

            auction.ReportPrice(1000, 98, "other bidder");
            application.HasShownSniperIsBidding(auction, 1000, 1098);

            auction.HasReceivedBid(1098, AuctionSniperDriver.SNIPER_ID);
                
            auction.AnnounceClosed();
            application.ShowsSniperHasLostAuction(auction);
        }

        [TestMethod]
        public void SniperWinsAnAuctionByBiddingHigher()
        {
            auction.StartSellingItem();

            application.StartBiddingIn(auction);
            auction.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);

            auction.ReportPrice(1000, 98, "other bidder");
            application.HasShownSniperIsBidding(auction, 1000, 1098);

            auction.HasReceivedBid(1098, AuctionSniperDriver.SNIPER_ID);

            auction.ReportPrice(1098, 97, AuctionSniperDriver.SNIPER_ID);
            application.HasShownSniperIsWinning(auction, 1098);

            auction.AnnounceClosed();
            application.ShowsSniperHasWonAuction(auction, 1098);
        }

        [TestMethod]
        public void SniperBidsForMultipleItems()
        {
            auction.StartSellingItem();
            auction2.StartSellingItem();

            application.StartBiddingIn(auction, auction2);

            auction.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);
            auction2.HasReceivedJoinRequestFrom(AuctionSniperDriver.SNIPER_ID);

            auction.ReportPrice(1000, 98, "other bidder");
            auction.HasReceivedBid(1098, AuctionSniperDriver.SNIPER_ID);

            auction2.ReportPrice(500, 21, "other bidder");
            auction2.HasReceivedBid(521, AuctionSniperDriver.SNIPER_ID);

            auction.ReportPrice(1098, 97, AuctionSniperDriver.SNIPER_ID);
            auction2.ReportPrice(521, 22, AuctionSniperDriver.SNIPER_ID);

            application.HasShownSniperIsWinning(auction, 1098);
            application.HasShownSniperIsWinning(auction2, 521);

            auction.AnnounceClosed();
            auction2.AnnounceClosed();

            application.ShowsSniperHasWonAuction(auction, 1098);
            application.ShowsSniperHasWonAuction(auction2, 521);
        }
    }
}
