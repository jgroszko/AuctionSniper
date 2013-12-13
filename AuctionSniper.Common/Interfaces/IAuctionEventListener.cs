using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common.Interfaces
{
    public enum PriceSource
    {
        FromSniper,
        FromOtherBidder
    }

    public interface IAuctionEventListener
    {
        void AuctionClosed();
        void CurrentPrice(int price, int increment, PriceSource source);
    }
}
