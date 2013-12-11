using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class SOLProtocol
    {
        public const string BID_COMMAND_FORMAT = "SOLVersion: 1.1; Event: BID; Price: {0};";
        public const string CLOSE_EVENT_FORMAT = "SOLVersion: 1.1; Event: CLOSE;";
        public const string JOIN_COMMAND_FORMAT = "SOLVersion: 1.1; Event: JOIN;";
        public const string PRICE_EVENT_FORMAT = "SOLVersion: 1.1; Event: PRICE;" +
            "CurrentPrice: {0}; Increment: {1}; Bidder: {2};";
    }
}
