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
            AuctionEvent ev = AuctionEvent.From(message.Body);
            
            switch(ev.Type)
            {
                case "CLOSE":
                    _listener.AuctionClosed();
                    break;
                case "PRICE":
                    _listener.CurrentPrice(ev.CurrentPrice, ev.Increment);
                    break;
                default:
                    throw new Exception("Invalid message");
            }
        }
    }
}
