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
        [TestMethod]
        public void NotifiesAuctionClosedWhenCloseMessageReceived()
        {
            Mock<IAuctionMessageListener> mock = new Mock<IAuctionMessageListener>();
            AuctionMessageTranslator amt = new AuctionMessageTranslator(mock.Object);
            
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = SOLProtocol.CLOSE_EVENT_FORMAT
            });

            mock.Verify(f => f.AuctionClosed(), Times.Once());
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceived()
        {
            Mock<IAuctionMessageListener> mock = new Mock<IAuctionMessageListener>();
            AuctionMessageTranslator amt = new AuctionMessageTranslator(mock.Object);

            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = string.Format(SOLProtocol.PRICE_EVENT_FORMAT,
                    0, 0, "other bidder")
            });

            mock.Verify(f => f.Price(0, 0, "other bidder"), Times.Once());
        }
    }
}
