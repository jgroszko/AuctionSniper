using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using FakeItEasy;
using jabber.protocol.client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        IAuctionEventListener auctionEventListener;
        AuctionMessageTranslator amt;

        [TestInitialize]
        public void Initialize()
        {
            auctionEventListener = A.Fake<IAuctionEventListener>();
            amt = new AuctionMessageTranslator(SNIPER_ID, auctionEventListener);
        }

        [TestMethod]
        public void NotifiesAuctionClosedWhenCloseMessageReceived()
        {
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = SOLProtocol.CLOSE_EVENT_FORMAT
            });

            A.CallTo(() => auctionEventListener.AuctionClosed()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromOtherBidder()
        {
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = string.Format(SOLProtocol.PRICE_EVENT_FORMAT,
                    0, 0, "other bidder")
            });

            A.CallTo(() => auctionEventListener.CurrentPrice(0, 0, PriceSource.FromOtherBidder)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromSniper()
        {
            amt.ProcessMessage(new Message(new XmlDocument())
            {
                Body = string.Format(SOLProtocol.PRICE_EVENT_FORMAT,
                    0, 0, SNIPER_ID)
            });

            A.CallTo(() => auctionEventListener.CurrentPrice(0, 0, PriceSource.FromSniper)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
