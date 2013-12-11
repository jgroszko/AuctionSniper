using jabber.client;
using jabber.protocol.client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionSniperTests
{
    public class SingleMessageListener
    {
        JabberClient _jc;

        MessageHandler _handler;

        AutoResetEvent _event = new AutoResetEvent(false);
        ConcurrentQueue<Message> _msg = new ConcurrentQueue<Message>();

        public SingleMessageListener(JabberClient jc)
        {
            _jc = jc;

            _handler = new MessageHandler(ProcessMessage);
            _jc.OnMessage += _handler;
        }

        public void ProcessMessage(object sender, Message msg)
        {
            if (!string.IsNullOrEmpty(msg.Body))
            {
                _msg.Enqueue(msg);
                _event.Set();
            }
        }

        public bool ReceivesAMessage()
        {
            Message result;
            if(_msg.TryDequeue(out result))
            {
                return true;
            }
            else
            {
                _event.WaitOne(TimeSpan.FromSeconds(10));
                return _msg.TryDequeue(out result);
            }
        }
    }
}
