using AuctionSniper.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public class AuctionMessageTranslator : IMessageListener
    {
        private IAuctionMessageListener _listener;

        public AuctionMessageTranslator(IAuctionMessageListener listener)
        {
            _listener = listener;
        }

        public void ProcessMessage(jabber.protocol.client.Message message)
        {
            var data = message.Body.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            var pairs = data.ToDictionary(d => d.Substring(0, d.IndexOf(":")).Trim(),
                d => d.Substring(d.IndexOf(":")+1).Trim());
            
            switch(pairs["Event"])
            {
                case "CLOSE":
                    _listener.AuctionClosed();
                    break;
                case "PRICE":
                    _listener.CurrentPrice(int.Parse(pairs["CurrentPrice"]),
                                    int.Parse(pairs["Increment"]));
                    break;
                default:
                    throw new Exception("Invalid message");
            }
        }
    }
}
