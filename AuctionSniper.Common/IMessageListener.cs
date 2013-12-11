using jabber.protocol.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public interface IMessageListener
    {
        void ProcessMessage(object sender, Message message);
    }
}
