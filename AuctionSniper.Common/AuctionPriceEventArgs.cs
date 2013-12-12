using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public delegate void AuctionPriceEventHandler(object sender, AuctionPriceEventArgs args);

    public class AuctionPriceEventArgs : EventArgs
    {
        public int CurrentPrice { get; set; }
        public int Increment { get; set; }
        public string Bidder { get; set; }
    }
}
