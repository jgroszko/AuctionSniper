using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class Auction
    {
        public virtual void Bid(int price)
        {
            XmppService.Message(string.Format(SOLProtocol.BID_COMMAND_FORMAT, price));
        }

        public Services.XmppService XmppService { get; set; }
    }
}
