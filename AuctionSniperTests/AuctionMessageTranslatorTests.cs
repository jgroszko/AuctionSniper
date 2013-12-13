using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using jabber.protocol.client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AuctionSniperTests
{
    [TestClass]
    public class AuctionMessageTranslatorTests
    {
        public const string SNIPER_ID = "sniper_id";

        Mock<IAuctionEventListener> mock;
        AuctionMessageTranslator amt;

        [TestInitialize]
        public void Initialize()
        {
            mock = new Mock<IAuctionEventListener>();
            amt = new AuctionMessageTranslator(SNIPER_ID, mock.Object);
        }

        [TestMethod]
        public void NotifiesAuctionClosedWhenCloseMessageReceived()
        {
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = SOLProtocol.CLOSE_EVENT_FORMAT
            });

            mock.Verify(f => f.AuctionClosed(), Times.Once());
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromOtherBidder()
        {
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = string.Format(SOLProtocol.PRICE_EVENT_FORMAT,
                    0, 0, "other bidder")
            });

            mock.Verify(f => f.CurrentPrice(0, 0, PriceSource.FromOtherBidder), Times.Once());
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromSniper()
        {
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = string.Format(SOLProtocol.PRICE_EVENT_FORMAT,
                    0, 0, SNIPER_ID)
            });

            mock.Verify(f => f.CurrentPrice(0, 0, PriceSource.FromSniper), Times.Once());
        }
    }
}
