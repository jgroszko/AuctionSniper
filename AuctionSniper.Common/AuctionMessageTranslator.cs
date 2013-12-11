using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class AuctionMessageTranslator : IMessageListener
    {
        public event EventHandler OnAuctionClose;

        public void ProcessMessage(object sender, jabber.protocol.client.Message message)
        {
            if(OnAuctionClose != null)
            {
                OnAuctionClose(this, new EventArgs());
            }
        }
    }
}
