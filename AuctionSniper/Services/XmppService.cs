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

namespace AuctionSniper.Services
{
    public class XmppService
    {
        public const string CONFIG_JID = "jid";
        public const string CONFIG_PASSWORD = "password";
        public const string CONFIG_HOST = "host";
        public const string XMPP_RESOURCE = "Auction Sniper";

        JabberClient _jc;

        public event EventHandler OnAuthenticated;
        public event MessageHandler OnMessage;

        public static XmppService _instance = null;
        public static XmppService Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new XmppService();
                }
                return _instance;
            }
        }

        protected XmppService()
        {
            _jc = new JabberClient();
            _jc.Resource = XMPP_RESOURCE;

            JID j = new JID(ConfigurationManager.AppSettings[CONFIG_JID]);
            _jc.User = j.User;
            _jc.Server = j.Server;
            _jc.Password = ConfigurationManager.AppSettings[CONFIG_PASSWORD];
            _jc.NetworkHost = ConfigurationManager.AppSettings[CONFIG_HOST];

            _jc.OnAuthenticate += new bedrock.ObjectHandler(Authenticated);
            _jc.OnMessage += new MessageHandler(Message);
        }

        public void Connect()
        {
            _jc.Connect();
        }

        public void Close()
        {
            _jc.Close();
        }

        public void Message(string to, string body)
        {
            _jc.Message(to, body);
        }

        protected void Authenticated(object sender)
        {
            if (OnAuthenticated != null)
            {
                OnAuthenticated(this, new EventArgs());
            }
        }

        protected void Message(object sender, Message message)
        {
            if(OnMessage != null &&
                !string.IsNullOrEmpty(message.Body))
            {
                OnMessage(this, message);
            }
        }
    }
}
