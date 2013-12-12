using jabber;
using jabber.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using jabber.protocol.client;
using AuctionSniper.Common;
using System.Threading;
using AuctionSniper.Common.Interfaces;

namespace AuctionSniper.Common.Services
{
    public class XmppService : IDisposable
    {
        public const string XMPP_RESOURCE = "Auction Sniper";

        JabberClient _jc;

        public IMessageListener _listener;

        public XmppService(string jid, string password, string host, IMessageListener listener)
        {
            _listener = listener;

            _jc = new JabberClient();
            _jc.Resource = XMPP_RESOURCE;
            _jc.AutoStartTLS = false;
            _jc.SSL = false;

            JID j = new JID(jid);
            _jc.User = j.User;
            _jc.Server = j.Server;
            _jc.Password = password;
            _jc.NetworkHost = host;

            _jc.OnMessage += new MessageHandler(Message);
        }

        public void Connect()
        {
            ManualResetEvent authenticatedEvent = new ManualResetEvent(false);

            _jc.OnAuthenticate += new bedrock.ObjectHandler(sender =>
            {
                authenticatedEvent.Set();
            });

            _jc.Connect();

            authenticatedEvent.WaitOne();
        }

        public void Dispose()
        {
            _jc.Close();
        }

        public void Message(string to, string body)
        {
            _jc.Message(to, body);
        }

        protected void Message(object sender, Message message)
        {
            if (!string.IsNullOrEmpty(message.Body))
            {
                _listener.ProcessMessage(message);
            }
        }
    }
}
