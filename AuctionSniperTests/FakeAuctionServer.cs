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

        private JabberClient _jc;
        private SingleMessageListener _listener;

        public FakeAuctionServer(string itemId)
        {
            _itemId = itemId;
            _jc = new JabberClient();
            _jc.NetworkHost = XMPP_HOSTNAME;

            _jc.OnAuthError += new ProtocolHandler((sender, ex) =>
            {
                Debug.WriteLine(ex.ToString());
            });
            _jc.OnError += new bedrock.ExceptionHandler((sender, ex) =>
            {
                Debug.WriteLine(ex.ToString());
            });
        }

        public void StartSellingItem()
        {
            JID j = new JID(string.Format(ITEM_ID_AS_LOGIN, ItemId));
            _jc.User = j.User;
            _jc.Server = j.Server;
            _jc.Password = AUCTION_PASSWORD;

            ManualResetEvent mse = new ManualResetEvent(false);
            _jc.OnAuthenticate += new bedrock.ObjectHandler(sender =>
            {
                mse.Set();

                _listener = new SingleMessageListener(_jc);
            });

            _jc.Connect();
            Assert.IsTrue(mse.WaitOne(TimeSpan.FromSeconds(10)));
        }

        public void Dispose()
        {
            _jc.Dispose();
        }

        public void hasReceivedJoinRequestFromSniper()
        {
            Assert.IsTrue(_listener.ReceivesAMessage());
        }

        internal void announceClosed()
        {
            _jc.Message("sniper@jgroszko-server", "Closed");
        }
    }
}
