using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using AuctionSniper.Common.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniperTests
{
    [TestClass]
    public class AuctionSniperServiceTests
    {
        Mock<ISniperListener> sniperListener;
        Mock<Auction> auction;
        AuctionSniperService sniper;

        [TestInitialize]
        public void Initialize()
        {
            sniperListener = new Mock<ISniperListener>();
            auction = new Mock<Auction>(string.Empty);
            sniper = new AuctionSniperService(auction.Object, sniperListener.Object);
        }

        [TestMethod]
        public void ReportsLostIfAuctionClosesImmediately()
        {
            sniperListener.Setup(f => f.SniperLost());

            sniper.AuctionClosed();

            sniperListener.Verify(f => f.SniperLost(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ReportsLostIfAuctionClosesWhenBidding()
        {
            var sequence = new MockSequence();
            sniperListener.InSequence(sequence).Setup(f => f.SniperBidding());
            sniperListener.InSequence(sequence).Setup(f => f.SniperLost());

            sniper.CurrentPrice(123, 45, PriceSource.FromOtherBidder);
            sniper.AuctionClosed();

            sniperListener.VerifyAll(); // TODO: Sequence doesn't actually work in Loose mode
        }

        [TestMethod]
        public void ReportsWonIfAuctionClosesWhenWinning()
        {
            var sequence = new MockSequence();
            sniperListener.InSequence(sequence).Setup(f => f.SniperWinning());
            sniperListener.InSequence(sequence).Setup(f => f.SniperWon());

            sniper.CurrentPrice(123, 45, PriceSource.FromSniper);
            sniper.AuctionClosed();

            sniperListener.Verify(f => f.SniperWinning(), Times.AtLeastOnce());
            sniperListener.Verify(f => f.SniperWon(), Times.Once());
        }

        [TestMethod]
        public void ReportsIsWinningWhenCurrentPriceComesFromSniper()
        {
            int price = 1001;
            int increment = 25;

            sniper.CurrentPrice(price, increment, PriceSource.FromSniper);

            sniperListener.Verify(f => f.SniperWinning(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
        {
            int price = 1001;
            int increment = 25;

            sniperListener.Setup(f => f.SniperBidding());
            auction.Setup(f => f.Bid(price + increment));

            sniper.CurrentPrice(price, increment, PriceSource.FromOtherBidder);

            sniperListener.Verify(f => f.SniperBidding(), Times.AtLeastOnce());
            auction.Verify(f => f.Bid(price + increment), Times.Once());
        }
    }
}
