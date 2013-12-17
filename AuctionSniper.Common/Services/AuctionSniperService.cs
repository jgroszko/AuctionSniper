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

        private bool _isWinning = false;

        public AuctionSniperService(Auction auction, string itemId, ISniperListener listener)
        {
            _auction = auction;
            _itemId = itemId;
            _listener = listener;
        }

        public void AuctionClosed()
        {
            if(_isWinning)
            {
                _listener.SniperWon();
            }
            else
            {
                _listener.SniperLost();
            }
        }

        public void CurrentPrice(int price, int increment, PriceSource priceSource)
        {
            _isWinning = priceSource == PriceSource.FromSniper;
            if(_isWinning)
            {
                _listener.SniperWinning();
            }
            else
            {
                int bid = price + increment;
                _auction.Bid(bid);
                _listener.SniperBidding(new SniperState(_itemId, price, bid));
            }
        }
    }
}
