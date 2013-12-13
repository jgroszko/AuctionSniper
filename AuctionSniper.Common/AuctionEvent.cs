using AuctionSniper.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class AuctionEvent
    {

        private Dictionary<string, string> Fields { get; set; }

        public static AuctionEvent From(string messageBody)
        {
            AuctionEvent ev = new AuctionEvent();

            var data = messageBody.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            ev.Fields = data.ToDictionary(d => d.Substring(0, d.IndexOf(":")).Trim(),
                d => d.Substring(d.IndexOf(":") + 1).Trim());

            return ev;
        }

        public string Type
        {
            get
            {
                return Fields["Event"];
            }
        }

        public int CurrentPrice
        {
            get
            {
                return int.Parse(Fields["CurrentPrice"]);
            }
        }

        public int Increment
        {
            get
            {
                return int.Parse(Fields["Increment"]);
            }
        }

        public string Bidder
        {
            get
            {
                return Fields["Bidder"];
            }
        }

        public PriceSource IsFrom(string _sniperId)
        {
            return _sniperId.Equals(Bidder) ? PriceSource.FromSniper : PriceSource.FromOtherBidder;
        }
    }
}
