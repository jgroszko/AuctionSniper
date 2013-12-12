using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class Auction
    {
        private string _auctionUser;

        public Services.XmppService XmppService { get; set; }

        public Auction(string auctionUser)
        {
            _auctionUser = auctionUser;
        }

        public virtual void Bid(int price)
        {
            XmppService.Message(_auctionUser, string.Format(SOLProtocol.BID_COMMAND_FORMAT, price));
        }

        public virtual void Join()
        {
            XmppService.Message(_auctionUser, string.Format(SOLProtocol.JOIN_COMMAND_FORMAT));
        }
    }
}
