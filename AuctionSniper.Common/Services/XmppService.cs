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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Auction));

        public const string XMPP_RESOURCE = "Auction Sniper";

        JabberClient _jc;

        public XmppService(string jid, string password, string host)
        {
            _jc = new JabberClient();
            _jc.Resource = XMPP_RESOURCE;
            _jc.AutoStartTLS = false;
            _jc.SSL = false;

            JID j = new JID(jid);
            _jc.User = j.User;
            _jc.Server = j.Server;
            _jc.Password = password;
            _jc.NetworkHost = host;
        }

        public void AddMessageHandler(IMessageListener listener, string user = null)
        {
            _jc.OnMessage += new MessageHandler((sender, message) =>
            {
                if(!string.IsNullOrEmpty(message.Body) &&
                    (user == null || user == message.From.Bare))
                {
                    log.Info(string.Format("Message from {0}: {1}", message.From.Bare, message.Body));

                    listener.ProcessMessage(message);
                }
            });
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
            log.Info(string.Format("Message to {0}: {1}", to, body));
            _jc.Message(to, body);
        }

        public string GetUser()
        {
            return _jc.JID.Bare;
        }
    }
}
