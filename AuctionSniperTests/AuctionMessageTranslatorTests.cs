using AuctionSniper.Common;
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
        [TestMethod]
        public void NotifiesAuctionClosedWhenCloseMessageReceived()
        {
            AuctionMessageTranslator amt = new AuctionMessageTranslator();
            ManualResetEvent mre = new ManualResetEvent(false);
            amt.OnAuctionClose += new EventHandler((sender, args) =>
                {
                    mre.Set();
                });

            amt.ProcessMessage(null, new Message(new XmlDocument())
            {
                Body = SOLProtocol.CLOSE_EVENT_FORMAT
            });

            Assert.IsTrue(mre.WaitOne(TimeSpan.FromSeconds(10)));
        }
    }
}
