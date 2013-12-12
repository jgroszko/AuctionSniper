using AuctionSniper.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common.Services
{
    public class AuctionSniperService : IAuctionMessageListener
    {
        private ISniperListener _listener;
        private Auction _auction;

        public AuctionSniperService(Auction auction, ISniperListener listener)
        {
            _auction = auction;
            _listener = listener;
        }

        public void AuctionClosed()
        {
            _listener.SniperLost();
        }

        public void CurrentPrice(int price, int increment)
        {
            _auction.Bid(price + increment);
            _listener.SniperBidding();
        }
    }
}
