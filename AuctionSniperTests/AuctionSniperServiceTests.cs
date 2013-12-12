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
        [TestMethod]
        public void ReportsLostWhenAuctionCloses()
        {
            Mock<ISniperListener> mock = new Mock<ISniperListener>();
            AuctionSniperService sniper = new AuctionSniperService(mock.Object);

            sniper.AuctionClosed();

            mock.Verify(f => f.SniperLost(), Times.Once());
        }
    }
}
