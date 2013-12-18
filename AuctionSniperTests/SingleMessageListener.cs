using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using jabber.client;
using jabber.protocol.client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionSniperTests
{
    public class SingleMessageListener : IMessageListener
    {
        AutoResetEvent _event = new AutoResetEvent(false);
        ConcurrentQueue<Message> _msg = new ConcurrentQueue<Message>();

        public string CurrentChat { get; set; }

        public Message ReceivesAMessage()
        {
            _event.WaitOne(TimeSpan.FromSeconds(10));

            Message result;
            _msg.TryDequeue(out result);
            return result;
        }

        public void ProcessMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Body))
            {
                if (message.From != null)
                {
                    CurrentChat = message.From.Bare;
                }

                _msg.Enqueue(message);
                _event.Set();
            }
        }
    }
}
