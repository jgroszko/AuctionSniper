using AuctionSniper.Common;
using AuctionSniper.Common.Services;
using bedrock;
using jabber;
using jabber.client;
using jabber.protocol;
using jabber.protocol.client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace AuctionSniperTests
{
    public class FakeAuctionServer : IDisposable
    {
        public const string ITEM_ID_AS_LOGIN = "auction-{0}@jgroszko-server";
        public const string AUCTION_RESOURCE = "Auction";
        public const string XMPP_HOSTNAME = "192.168.101.119";
        public const string AUCTION_PASSWORD = "auction";

        private string _itemId;
        public string ItemId
        {
            get
            {
                return _itemId;
            }
        }

        private XmppService _xmpp;
        private SingleMessageListener _listener = new SingleMessageListener();

        public FakeAuctionServer(string itemId)
        {
            _itemId = itemId;

            _xmpp = new XmppService(
                string.Format(ITEM_ID_AS_LOGIN, ItemId),
                AUCTION_PASSWORD,
                XMPP_HOSTNAME,
                _listener);
        }

        public void StartSellingItem()
        {
            _xmpp.Connect();
        }

        public void Dispose()
        {
            _xmpp.Dispose();
        }

        public void HasReceivedJoinRequestFrom(string sniperId)
        {
            Message msg = _listener.ReceivesAMessage();
            Assert.AreEqual(sniperId, msg.From.Bare);
            Assert.AreEqual(SOLProtocol.JOIN_COMMAND_FORMAT, msg.Body);
        }

        public void AnnounceClosed()
        {
            _xmpp.Message(_listener.CurrentChat, SOLProtocol.CLOSE_EVENT_FORMAT);
        }

        public void ReportPrice(int price, int increment, string bidder)
        {
            _xmpp.Message(_listener.CurrentChat,
                string.Format(
                    SOLProtocol.PRICE_EVENT_FORMAT,
                    price, increment, bidder));
        }

        public void HasReceivedBid(int bid, string sniperId)
        {
            Message msg = _listener.ReceivesAMessage();
            Assert.AreEqual(sniperId, msg.From.Bare);
            Assert.AreEqual(SOLProtocol.BID_COMMAND_FORMAT, bid);
        }
    }
}
