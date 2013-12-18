using AuctionSniper.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common.Services
{
    public class AuctionSniperService : IAuctionEventListener
    {
        private ISniperListener _listener;
        private Auction _auction;
        private string _itemId;

        private SniperSnapshot _snapshot;

        public AuctionSniperService(string itemId, Auction auction, ISniperListener listener)
        {
            _itemId = itemId;
            _auction = auction;
            _listener = listener;
            _snapshot = SniperSnapshot.Joining(itemId);
        }

        private void NotifyChange()
        {
            _listener.SniperStateChanged(_snapshot);
        }

        public void AuctionClosed()
        {
            _snapshot = _snapshot.Closed();
            NotifyChange();
        }

        public void CurrentPrice(int price, int increment, PriceSource priceSource)
        {
            switch(priceSource)
            {
                case PriceSource.FromSniper:
                    _snapshot = _snapshot.Winning(price);
                    break;
                case PriceSource.FromOtherBidder:
                    int bid = price + increment;
                    _auction.Bid(bid);
                    _snapshot = _snapshot.Bidding(price, bid);
                    break;
            }
            NotifyChange();
        }
    }
}
