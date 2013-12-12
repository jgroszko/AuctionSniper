using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class AuctionMessageTranslator : IMessageListener
    {
        public event EventHandler OnAuctionClose;
        public event AuctionPriceEventHandler OnAuctionPrice;

        public void ProcessMessage(object sender, jabber.protocol.client.Message message)
        {
            var data = message.Body.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            var pairs = data.ToDictionary(d => d.Substring(0, d.IndexOf(":")).Trim(),
                d => d.Substring(d.IndexOf(":")+1).Trim());
            
            switch(pairs["Event"])
            {
                case "CLOSE":
                    if(OnAuctionClose != null)
                    {
                        OnAuctionClose(this, EventArgs.Empty);
                    }
                    break;
                case "PRICE":
                    if(OnAuctionPrice != null)
                    {
                        OnAuctionPrice(this, new AuctionPriceEventArgs()
                            {
                                CurrentPrice = int.Parse(pairs["CurrentPrice"]),
                                Increment = int.Parse(pairs["Increment"]),
                                Bidder = pairs["Bidder"]
                            });
                    }
                    break;
                default:
                    throw new Exception("Invalid message");
            }
        }
    }
}
