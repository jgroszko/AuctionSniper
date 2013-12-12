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
        public void ReportsLostWhenAuctionCloses()
        {
            sniper.AuctionClosed();

            sniperListener.Verify(f => f.SniperLost(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
        {
            int price = 1001;
            int increment = 25;

            sniper.CurrentPrice(price, increment);

            sniperListener.Verify(f => f.SniperBidding(), Times.AtLeastOnce());
            auction.Verify(f => f.Bid(price + increment), Times.Once());
        }
    }
}
