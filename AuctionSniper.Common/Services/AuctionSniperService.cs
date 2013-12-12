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

        public AuctionSniperService(ISniperListener listener)
        {
            _listener = listener;
        }

        public void AuctionClosed()
        {
            _listener.SniperLost();
        }

        public void Price(int bid, int increment, string bidder)
        {
        }
    }
}
